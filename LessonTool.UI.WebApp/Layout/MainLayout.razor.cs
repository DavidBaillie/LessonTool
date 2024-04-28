using LessonTool.UI.Infrastructure.Interfaces;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;

namespace LessonTool.UI.WebApp.Layout;

public partial class MainLayout : IDisposable
{
    [Inject]
    private IAuthenticationStateHandler authenticationHandler {  get; set; }

    [Inject]
    private NavigationManager navigationManager { get; set; }

    private JwtSecurityToken userToken;
    private string username = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadUserDataFromAccesstoken();

        authenticationHandler.OnLoginStateChangedAsync += ReloadUserDataFromAccesstoken;
    }

    public void Dispose()
    {
        authenticationHandler.OnLoginStateChangedAsync -= ReloadUserDataFromAccesstoken;
    }

    private async Task ReloadUserDataFromAccesstoken()
    {
        userToken = await authenticationHandler.GetSecurityTokenAsync(CancellationToken.None);
        username = userToken.GetUsernameClaim();
    }

    private async Task LogoutAsync()
    {
        await authenticationHandler.TryLogoutAsync(CancellationToken.None);
    }
}