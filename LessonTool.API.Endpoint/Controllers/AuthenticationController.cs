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
                    return await GenerateAccountTokens(loginRequest, cancellationToken);
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
        private async Task<AccessTokensModel> GenerateAccountTokens(LoginRequestModel request, CancellationToken cancellationToken)
        {
            //Check to see if we have a user
            var user = await _userAccounts.GetAccountByUsernameAsync(request.Username, cancellationToken);
            if (user == null)
                throw new AuthenticationFailureException("User does not exist, cannot generate token!");

            //Check that the password matches the salted version
            var hashedPassword = _hashService.HashStringWithSalt(request.HashedPassword, Encoding.UTF8.GetBytes(user.PasswordSalt));
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
            await _loginSessions.CreateAsync(
                new UserLoginSession()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresDateTime = expires,
                    UserAccountId = user.Id,
                }, cancellationToken);

            return new AccessTokensModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }
    }
}