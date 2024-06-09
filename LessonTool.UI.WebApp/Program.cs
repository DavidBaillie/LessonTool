using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Services;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Application.Repositories;
using LessonTool.UI.Infrastructure.Authentication;
using LessonTool.UI.Infrastructure.Browser;
using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.HttpServices;
using LessonTool.UI.Infrastructure.Interfaces;
using LessonTool.UI.WebApp.Middleware;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace LessonTool.UI.WebApp
{
    public class Program
    {
        private const string apiEndpoint = "https://localhost:44360";
        //private const string apiEndpoint = "https://yg-api.azurewebsites.net/";

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //Regular Client with Auth
            builder.Services
                .AddHttpClient(ApiEndpointConstants.CommonApiClientName, options =>
                    {
                        options.BaseAddress = new Uri(apiEndpoint);
                    })
                .AddHttpMessageHandler<AuthenticationTokenClientMiddleware>();

            //Special client with no auth
            builder.Services
                .AddHttpClient(ApiEndpointConstants.AuthenticationApiClientName, options =>
                    {
                        options.BaseAddress = new Uri(apiEndpoint);
                    });


            builder.Services.AddSingleton<IAuthenticationStateHandler, AuthenticationStateHandler>();

            builder.Services.AddTransient<IFullLessonRepository, FullLessonRepository>();
            builder.Services.AddTransient<ILessonRepository, LessonEndpoint>();
            builder.Services.AddTransient<ISectionRepository, SectionEndpoint>();
            builder.Services.AddTransient<IAuthenticationEndpoint, AuthenticationEndpoint>();
            builder.Services.AddTransient<AuthenticationTokenClientMiddleware>();
            
            builder.Services.AddTransient<IHashService, HashService>();
            builder.Services.AddTransient<IPersistentStorage, BrowserLocalStorageProvider>();
            builder.Services.AddTransient<IBrowserLocalStorage, BrowserLocalStorageProvider>();
            builder.Services.AddTransient<IBrowserSessionStorage, BrowserSessionStorageProvider>();

            await builder.Build().RunAsync();
        }
    }
}
