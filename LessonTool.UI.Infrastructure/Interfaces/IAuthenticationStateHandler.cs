﻿namespace LessonTool.UI.Infrastructure.Interfaces
{
    public interface IAuthenticationStateHandler
    {
        //string AccessToken { get; }

        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
        Task<bool> TryLoginUsingCredentialsAsync(string username, string password, bool rememberSession, CancellationToken cancellationToken);
        Task<bool> TryLogout(CancellationToken cancellationToken);
    }
}