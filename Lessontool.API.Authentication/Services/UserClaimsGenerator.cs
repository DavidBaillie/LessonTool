using Lessontool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;
using System.Security.Claims;

namespace Lessontool.API.Authentication.Services;

public class UserClaimsGenerator(IRepository<AuthenticatedUser> userRepository)
{
    public List<Claim> GenerateUserClaims(string username)
    {
        return default;
    }
}
