using Lessontool.API.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[Route("api/authentication")]
public class UserAutheticationController : ControllerBase
{
    [HttpPost("/login")]
    public async Task<AccessTokens> LoginAsync(UserLoginAttempt login, CancellationToken cancellationToken)
    {
        return default(AccessTokens);
    }

    [HttpPut("/refresh")]
    public async Task<AccessTokens> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
    {
        return default(AccessTokens);
    }
}
