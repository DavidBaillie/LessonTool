using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LessonTool.API.Authentication.Services;

public class TokenGenerationService(IConfiguration _configuration) : ITokenGenerationService
{
    public SigningCredentials CreateSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("jwt")["key"]);
        return new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);
    }

    public List<Claim> CreateUserClaims(UserAccount user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.AccountType)
        };
    }

    public JwtSecurityToken CreateJwtSecurityToken(SigningCredentials credentials, List<Claim> claims, int expiresAfterMinutes)
    {
        return new JwtSecurityToken(
            issuer: _configuration.GetSection("jwt")["issuer"],
            audience: _configuration.GetSection("jwt")["audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresAfterMinutes),
            signingCredentials: credentials);
    }

    public string WriteSecurityToken(JwtSecurityToken token)
    {
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken()
    {
        using var generator = RandomNumberGenerator.Create();

        var number = new byte[32];
        generator.GetBytes(number);
        return Convert.ToBase64String(number);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("jwt")["key"]);

        var tokenParams = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = false,
            ValidIssuer = _configuration.GetSection("jwt")["issuer"],
            ValidAudience = _configuration.GetSection("jwt")["audience"],
        };

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, tokenParams, out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}