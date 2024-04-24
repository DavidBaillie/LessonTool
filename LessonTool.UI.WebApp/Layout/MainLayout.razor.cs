using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LessonTool.UI.WebApp.Layout;

public partial class MainLayout
{
    private string username = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            var token = await authHandler.GetAccessTokenAsync(CancellationToken.None);

            var name = new JwtSecurityToken(token).Claims.First(x => x.Type == ClaimTypes.Name).Value;
            if (name != "Anonymous")
            {
                username = name;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
