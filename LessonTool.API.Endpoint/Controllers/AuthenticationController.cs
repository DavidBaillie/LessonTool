using LessonTool.API.Endpoint.Models;
using LessonTool.API.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController(CosmosDbContext context) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IResult> AuthenticateAsync([FromBody] LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new[] { new Claim("Username", loginRequest.Username) }, BearerTokenDefaults.AuthenticationScheme));

            
            var result = Results.SignIn(claimsPrincipal).ExecuteAsync(HttpContext);
            return default;
        }

        [HttpPut("refresh")]
        public async Task<ActionResult> RefreshTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            return default;
        }

        [HttpPost("logout")]
        public async Task<ActionResult> SignOutAsync(CancellationToken cancellationToken)
        {
            return default;
        }

        [HttpGet("read")]
        public async Task<string> ReadClaims()
        {
            var x = context.DataProtectionKeys.ToList();

            var principal = HttpContext.User;

            var username = principal.Claims.FirstOrDefault(x => x.Type == "Username");

            if (username == null)
            {
                return string.Join(", ", principal.Claims.Select(x => x.Value));
            }
            else
            {
                return username.Value;
            }
        }
    }
}
