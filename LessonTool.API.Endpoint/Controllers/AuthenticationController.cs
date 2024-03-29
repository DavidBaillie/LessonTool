using LessonTool.API.Authentication.Exceptions;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController(IUserAccountRepository _userAccounts, ILoginSessionRepository _loginSessions, ITokenGenerationService _tokenGenerator,
        IHashService _hashService) 
        : ControllerBase
    {
        private const int TokenExpiresMinutes = 120;
        private const int TokenExpiresMinutesThreshold = -5;

        [HttpPost("login")]
        public async Task<ActionResult<AccessTokensResponseModel>> AuthenticateAsync([FromBody] LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                //Quick safety check to filter out bots scanning endpoints
                if (loginRequest.RequestToken != TokenConstants.AuthenticationRequestToken || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.HashedPassword))
                    return Unauthorized();

                //Anonymous account for readonly
                if (loginRequest.Username == TokenConstants.AnonymousAccountToken.ToString() && loginRequest.HashedPassword == TokenConstants.AnonymousAccountToken.ToString())
                    return await ProcessAnonymousLoginRequest(cancellationToken);
                //Actual account for read/write
                else
                {
                    //Check to see if we have a user
                    var user = await _userAccounts.GetAccountByUsernameAsync(loginRequest.Username, cancellationToken);
                    if (user == null)
                        return Unauthorized();

                    //User needs to reset password
                    if (string.IsNullOrWhiteSpace(user.Password))
                        return Conflict();

                    return await ProcessUserAccountLoginRequest(user, loginRequest, cancellationToken);
                }
            }
            catch (AuthenticationFailureException authEx)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPut("refresh")]
        public async Task<ActionResult<AccessTokensResponseModel>> RefreshTokensAsync([FromBody]RefreshTokensRequestModel tokens, CancellationToken cancellationToken)
        {
            if (tokens is null || tokens.RequestToken != TokenConstants.AuthenticationRequestToken)
                return Unauthorized();

            try
            {
                //Verify this API made the token and it's valid
                var principle = _tokenGenerator.GetPrincipalFromExpiredToken(tokens.Token);
                var username = principle.Identity.Name;

                //Process the token for a new session
                if (username == "Anonymous")
                    return await ProcessAnonymousRefreshRequest(tokens, cancellationToken);
                else
                    return await ProcessUserAccountRefreshRequest(username, tokens, cancellationToken);
            }
            catch (NotExpiredException)
            {
                return StatusCode(425);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword()
        {
            return default;
        }

        [HttpPost("test")]
        public async Task<ActionResult<string>> CreateFakeUser()
        {
            byte[] salt = _hashService.CreateSalt();
            string encodedSalt = Convert.ToBase64String(salt);
            string password = _hashService.HashString("password");
            string hashedPassword = _hashService.HashStringWithSalt(password, salt);

            var user = new UserAccount()
            {
                AccountType = "Admin",
                Username = "test",
                Password = hashedPassword,
                PasswordSalt = encodedSalt,
                PasswordResetToken = ""
            };

            await _userAccounts.CreateAsync(user);

            return Ok($"{password} // {encodedSalt}");
        }

        /// <summary>
        /// Generates a token set for the anonymous user to read only with limit access
        /// </summary>
        private async Task<AccessTokensResponseModel> ProcessAnonymousLoginRequest(CancellationToken cancellationToken)
        {
            //Create tokens
            var tokens = CreateAnonymousAccessTokens(TokenExpiresMinutes);

            //Save the session to the db
            await _loginSessions.CreateAsync(
                new UserLoginSession()
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    ExpiresDateTime = tokens.Expires,
                    UserAccountId = Guid.Empty,
                }, cancellationToken);

            return tokens;
        }

        /// <summary>
        /// Handles taking an expired access token and a refresh token to create a new pair if the state data is correct
        /// </summary>
        /// <param name="model">Refresh tokens model</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationFailureException"></exception>
        /// <exception cref="NotExpiredException"></exception>
        private async Task<AccessTokensResponseModel> ProcessAnonymousRefreshRequest(RefreshTokensRequestModel model, CancellationToken cancellationToken)
        {
            //Grab session for anonymous user
            var loginSession = (await _loginSessions.GetSessionByUserIdAsync(Guid.Empty.ToString(), model.RefreshToken, cancellationToken))
                ?? throw new AuthenticationFailureException("No login sessions for that user");

            //Check if token expired
            if (loginSession.ExpiresDateTime.AddMinutes(TokenExpiresMinutesThreshold) > DateTime.UtcNow)
                throw new NotExpiredException("Token not expired!");

            //Create new tokens for user
            var tokens = CreateAnonymousAccessTokens(TokenExpiresMinutesThreshold);

            //Save new tokens for this session
            loginSession.RefreshToken = tokens.RefreshToken;
            loginSession.AccessToken = tokens.AccessToken;
            loginSession.ExpiresDateTime = tokens.Expires;
            await _loginSessions.UpdateAsync(loginSession, cancellationToken);

            return tokens;
        }

        /// <summary>
        /// Generates a token set for request user if validation passes
        /// </summary>
        /// <param name="request">Login request</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationFailureException"></exception>
        private async Task<AccessTokensResponseModel> ProcessUserAccountLoginRequest(UserAccount user, LoginRequestModel request, CancellationToken cancellationToken)
        {
            var hashedPassword = _hashService.HashStringWithSalt(request.HashedPassword, Convert.FromBase64String(user.PasswordSalt));
            if (hashedPassword != user.Password)
                throw new AuthenticationFailureException("Password mismatch, login failed");

            //Create tokens
            var tokens = CreateUserAccessTokens(user, TokenExpiresMinutes);

            //Save the session to the db
            var session = new UserLoginSession()
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresDateTime = tokens.Expires,
                UserAccountId = user.Id,
            };
            await _loginSessions.CreateAsync(session, cancellationToken);

            return tokens;
        }

        /// <summary>
        /// Handles taking an expired access token and a refresh token to create a new pair if the state data is correct
        /// </summary>
        /// <param name="username">Username of user to refresh</param>
        /// <param name="model">Token model</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationFailureException"></exception>
        /// <exception cref="NotExpiredException"></exception>
        private async Task<AccessTokensResponseModel> ProcessUserAccountRefreshRequest(string username, RefreshTokensRequestModel model, CancellationToken cancellationToken)
        {
            //Grab user account
            var user = (await _userAccounts.GetAccountByUsernameAsync(username, cancellationToken))
                ?? throw new AuthenticationFailureException("No user with that name exists!");

            //Grab session for user
            var loginSession = (await _loginSessions.GetSessionByUserIdAsync(user.Id.ToString(), model.RefreshToken, cancellationToken))
                ?? throw new AuthenticationFailureException("No login sessions for that user");

            //Check if token expired
            if (loginSession.ExpiresDateTime.AddMinutes(TokenExpiresMinutesThreshold) > DateTime.UtcNow)
                throw new NotExpiredException("Token not expired!");

            //Create new tokens for user
            var tokens = CreateUserAccessTokens(user, TokenExpiresMinutesThreshold);
            
            //Save new tokens for this session
            loginSession.RefreshToken = tokens.RefreshToken;
            loginSession.AccessToken = tokens.AccessToken;
            loginSession.ExpiresDateTime = tokens.Expires;
            await _loginSessions.UpdateAsync(loginSession, cancellationToken);

            return tokens;
        }

        private AccessTokensResponseModel CreateUserAccessTokens(UserAccount user, int expiresMinutes)
        {
            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);
            var refreshToken = _tokenGenerator.CreateRefreshToken();
            var accessToken = _tokenGenerator.WriteSecurityToken(
                _tokenGenerator.CreateJwtSecurityToken(
                    _tokenGenerator.CreateSigningCredentials(),
                    _tokenGenerator.CreateUserClaims(user), expiresMinutes));

            return new AccessTokensResponseModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }

        private AccessTokensResponseModel CreateAnonymousAccessTokens(int expiresMinutes)
        {
            var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);
            var refreshToken = _tokenGenerator.CreateRefreshToken();
            var accessToken = _tokenGenerator.WriteSecurityToken(
                _tokenGenerator.CreateJwtSecurityToken(
                    _tokenGenerator.CreateSigningCredentials(),
                    _tokenGenerator.CreateAnonymousClaims(), expiresMinutes));

            return new AccessTokensResponseModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }
    }
}