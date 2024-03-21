using Azure.Core;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Text;
using System.Threading;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController(IUserAccountRepository _userAccounts, ILoginSessionRepository _loginSessions, ITokenGenerationService _tokenGenerator,
        IHashService _hashService) 
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<AccessTokensModel>> AuthenticateAsync([FromBody] LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                //Quick safety check to filter out bots scanning endpoints
                if (loginRequest.RequestToken != TokenConstants.LoginRequestToken || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.HashedPassword))
                    return Unauthorized();

                //Anonymous account for readonly
                if (loginRequest.Username == TokenConstants.AnonymousAccountToken.ToString() && loginRequest.HashedPassword == TokenConstants.AnonymousAccountToken.ToString())
                    return await GenerateAnonymousTokens(cancellationToken);
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

                    return await GenerateAccountTokens(user, loginRequest, cancellationToken);
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
        public async Task<ActionResult> RefreshTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            return default;
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
        private async Task<AccessTokensModel> GenerateAnonymousTokens(CancellationToken cancellationToken)
        {
            //Create tokens
            var expires = DateTime.UtcNow.AddMinutes(120);
            var refreshToken = _tokenGenerator.CreateRefreshToken();
            var accessToken = _tokenGenerator.WriteSecurityToken(
                _tokenGenerator.CreateJwtSecurityToken(
                    _tokenGenerator.CreateSigningCredentials(),
                    _tokenGenerator.CreateAnonymousClaims(), 120));

            //Save the session to the db
            await _loginSessions.CreateAsync(
                new UserLoginSession()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresDateTime = expires,
                    UserAccountId = Guid.Empty,
                }, cancellationToken);

            return new AccessTokensModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }

        /// <summary>
        /// Generates a token set for request user if validation passes
        /// </summary>
        /// <param name="request">Login request</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns></returns>
        /// <exception cref="AuthenticationFailureException"></exception>
        private async Task<AccessTokensModel> GenerateAccountTokens(UserAccount user, LoginRequestModel request, CancellationToken cancellationToken)
        {
            //Check that the password matches the salted version
            var recoded = Convert.ToBase64String(Convert.FromBase64String(user.PasswordSalt));

            var hashedPassword = _hashService.HashStringWithSalt(request.HashedPassword, Convert.FromBase64String(user.PasswordSalt));
            if (hashedPassword != user.Password)
                throw new AuthenticationFailureException("Password mismatch, login failed");

            //Create tokens
            var expires = DateTime.UtcNow.AddMinutes(120);
            var refreshToken = _tokenGenerator.CreateRefreshToken();
            var accessToken = _tokenGenerator.WriteSecurityToken(
                _tokenGenerator.CreateJwtSecurityToken(
                    _tokenGenerator.CreateSigningCredentials(),
                    _tokenGenerator.CreateUserClaims(user), 120));

            //Save the session to the db
            var session = new UserLoginSession()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresDateTime = expires,
                UserAccountId = user.Id,
            };
            await _loginSessions.CreateAsync(session, cancellationToken);

            return new AccessTokensModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }
    }
}