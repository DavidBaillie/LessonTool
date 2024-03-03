using System.Security.Claims;

namespace LessonTool.API.Endpoint.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(List<Claim> claims = null);
    }
}