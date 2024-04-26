using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.UI.WebApp.Pages;

public partial class LoginPage
{
    [Inject]
    private IAuthenticationStateHandler authenticationStateHandler {  get; set; }

    private string localTokenUsername = "Anonymous";

    private string username;
    private string password;
    private bool rememberMe;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        localTokenUsername = await GetUsernameFromAccessTokenAsync();
    }

    private async Task OnLoginButtonPressed()
    {
        if (await authenticationStateHandler.TryLoginUsingCredentialsAsync(username, password, rememberMe, CancellationToken.None))
        {
            username = "";
            password = "";

            localTokenUsername = await GetUsernameFromAccessTokenAsync();
            Console.WriteLine($"Logged in as {localTokenUsername}");
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
