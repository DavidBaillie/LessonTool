using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.UI.WebApp.Extensions;

public static class JwtSecurityTokenExtensions
{
    public static string GetUsernameClaim(this JwtSecurityToken token)
    {
        return token?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }

    public static IEnumerable<string> GetRoleClaims(this JwtSecurityToken token)
    {
        if (token == null) 
            return Enumerable.Empty<string>();

        return token.Claims
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();
    }
}
