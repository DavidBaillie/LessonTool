﻿
using System.IdentityModel.Tokens.Jwt;

namespace LessonTool.UI.Infrastructure.Interfaces
{
    public interface IAuthenticationStateHandler
    {
        //event Action OnLoginStateChanged;
        event Func<Task> OnLoginStateChangedAsync;

        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
        Task<JwtSecurityToken> GetSecurityTokenAsync(CancellationToken cancellationToken);
        Task<bool> TryLoginUsingCredentialsAsync(string username, string password, bool rememberSession, CancellationToken cancellationToken);
        Task<bool> TryLogoutAsync(CancellationToken cancellationToken);
    }
}