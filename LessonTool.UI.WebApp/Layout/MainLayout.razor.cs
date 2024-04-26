using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.UI.WebApp.Layout;

public partial class MainLayout : IDisposable
{
    [Inject]
    private IAuthenticationStateHandler authenticationHandler {  get; set; }

    private string username = string.Empty;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        authenticationHandler.OnLoginStateChanged += OnLoginStateChanged;

        try
        {
            await GetUsernameFromAccessTokenAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public void Dispose()
    {
        authenticationHandler.OnLoginStateChanged -= OnLoginStateChanged;
    }

    private void OnLoginStateChanged() => GetUsernameFromAccessTokenAsync().GetAwaiter().GetResult();
    private async Task GetUsernameFromAccessTokenAsync()
    {
        var token = await authenticationHandler.GetAccessTokenAsync(CancellationToken.None);

        var name = new JwtSecurityToken(token).Claims.First(x => x.Type == ClaimTypes.Name).Value;
        if (name != "Anonymous")
        {
            username = name;
        }
    }
}
