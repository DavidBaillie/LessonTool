using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController(IUserAccountRepository _userAccounts, ILoginSessionRepository _loginSessions, ITokenGenerationService _tokenGenerator) 
        : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<AccessTokensModel>> AuthenticateAsync([FromBody] LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            //Quick safety check to filter out bots scanning endpoints
            if (loginRequest.RequestToken != TokenConstants.LoginRequestToken || string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.HashedPassword))
                return Unauthorized();

            var user = await _userAccounts.GetAccountByUsernameAsync(loginRequest.Username, cancellationToken);

            if (user == null) 
                return Unauthorized();

            var expires = DateTime.UtcNow.AddMinutes(120);
            var refreshToken = _tokenGenerator.CreateRefreshToken();
            var accessToken = _tokenGenerator.WriteSecurityToken(
                _tokenGenerator.CreateJwtSecurityToken(
                    _tokenGenerator.CreateSigningCredentials(),
                    _tokenGenerator.CreateUserClaims(user), 120));

            await _loginSessions.CreateAsync(
                new UserLoginSession() 
                { 
                    AccessToken = accessToken, 
                    RefreshToken = refreshToken,
                    ExpiresDateTime = expires,
                    UserAccountId = user.Id,
                });

            return default;
        }

        [HttpPut("refresh")]
        public async Task<ActionResult> RefreshTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            return default;
        }

        private async Task GenerateAnonymousTokens()
        {

        }

        private async Task GenerateAccountTokens(UserAccount userAccount)
        {

        }
    }
}