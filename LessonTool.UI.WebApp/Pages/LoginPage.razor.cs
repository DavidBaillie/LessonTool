using LessonTool.UI.Infrastructure.Interfaces;
using LessonTool.UI.WebApp.Components.Common;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.UI.WebApp.Pages;

public partial class LoginPage
{
    [Inject]
    private IAuthenticationStateHandler authenticationStateHandler {  get; set; }

    [Inject]
    private NavigationManager navigationManager { get; set; }

    private InlineErrorDisplay inlineErrorComponent;

    private string localTokenUsername = "Anonymous";

    private string username;
    private string password;
    private bool rememberMe = true;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        localTokenUsername = (await authenticationStateHandler.GetSecurityTokenAsync(CancellationToken.None)).GetUsernameClaim();
    }

    private async Task OnLoginButtonPressed(Action OnComplete)
    {
        if (await authenticationStateHandler.TryLoginUsingCredentialsAsync(username, password, rememberMe, CancellationToken.None))
        {
            username = "";
            password = "";

            navigationManager.NavigateTo("/");
        }
        else
        {
            Console.WriteLine($"Failed to login!");
            inlineErrorComponent.DisplayError("Login failed!", 5);
        }

        await Task.Delay(5000);
        OnComplete?.Invoke();
    }

    private async Task<string> GetUsernameFromAccessTokenAsync()
    {
        return new JwtSecurityToken(await authenticationStateHandler.GetAccessTokenAsync(CancellationToken.None)).Claims
            .First(x => x.Type == ClaimTypes.Name).Value;
    }
}
