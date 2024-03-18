using LessonTool.API.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.API.Authentication.Interfaces
{
    public interface ITokenGenerationService
    {
        JwtSecurityToken CreateJwtSecurityToken(SigningCredentials credentials, List<Claim> claims, int expiresAfterMinutes);
        string CreateRefreshToken();
        SigningCredentials CreateSigningCredentials();
        List<Claim> CreateUserClaims(UserAccount user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}