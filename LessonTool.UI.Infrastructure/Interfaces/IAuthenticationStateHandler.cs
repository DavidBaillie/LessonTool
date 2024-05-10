
using System.IdentityModel.Tokens.Jwt;

namespace LessonTool.UI.Infrastructure.Interfaces
{
    public interface IAuthenticationStateHandler
    {
        event Func<Task> OnLoginStateChangedAsync;

        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
        Task<JwtSecurityToken> GetSecurityTokenAsync(CancellationToken cancellationToken = default);
        Task<bool> TryLoginUsingCredentialsAsync(string username, string password, bool rememberSession, CancellationToken cancellationToken = default);
        Task<bool> TryLogoutAsync(CancellationToken cancellationToken = default);
    }
}