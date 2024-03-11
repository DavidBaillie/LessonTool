using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("login")]
        public async Task<IResult> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, BearerTokenDefaults.AuthenticationScheme));

            return Results.SignIn(claimsPrincipal);
        }

        [HttpGet("refresh")]
        public async Task<ActionResult> RefreshTokensAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            return default;
        }

        [HttpPut]
        public async Task<ActionResult> SignOutAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}
