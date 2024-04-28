using LessonTool.UI.Infrastructure.Interfaces;
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

    private string localTokenUsername = "Anonymous";

    private string username;
    private string password;
    private bool rememberMe = true;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        localTokenUsername = (await authenticationStateHandler.GetSecurityTokenAsync(CancellationToken.None)).GetUsernameClaim();
    }

    private async Task OnLoginButtonPressed()
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
        }
    }

    private async Task<string> GetUsernameFromAccessTokenAsync()
    {
        return new JwtSecurityToken(await authenticationStateHandler.GetAccessTokenAsync(CancellationToken.None)).Claims
            .First(x => x.Type == ClaimTypes.Name).Value;
    }
}
