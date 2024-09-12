using LessonTool.API.Endpoint.Test.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LessonTool.API.Endpoint.Test.Configuration;

internal class EndpointWebApplicationFactory : WebApplicationFactory<ITestMarker>
{
    public WebApplicationFactory<ITestMarker> CreateFromTestOptions(TestIngOptions options)
    {
        var claimsProvider = ClaimsProvider.CreateTestClaims(options.UserType);

        return this.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Test", op => { });

                services.AddScoped<ClaimsProvider>(_ => claimsProvider);

                if (options.UseInMemoryDatabase)
                {
                    
                }
            });
        });
    }
}
