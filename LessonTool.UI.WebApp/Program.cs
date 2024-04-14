using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Services;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Application.Repositories;
using LessonTool.UI.Infrastructure.Authentication;
using LessonTool.UI.Infrastructure.Browser;
using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.HttpServices;
using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace LessonTool.UI.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient(ApiEndpointConstants.CommonApiClientName, options =>
            {
                options.BaseAddress = new Uri("https://localhost:44360");
            });


            builder.Services.AddScoped<IAuthenticationStateHandler, AuthenticationStateHandler>();
            builder.Services.AddScoped<IFullLessonRepository, FullLessonRepository>();
            builder.Services.AddScoped<ILessonRepository, LessonEndpoint>();
            builder.Services.AddScoped<ISectionRepository, SectionEndpoint>();
            builder.Services.AddScoped<IAuthenticationEndpoint, AuthenticationEndpoint>();
            
            builder.Services.AddTransient<IHashService, HashService>();
            builder.Services.AddTransient<IPersistentStorage, BrowserLocalStorageProvider>();
            builder.Services.AddTransient<IBrowserLocalStorage, BrowserLocalStorageProvider>();
            builder.Services.AddTransient<IBrowserSessionStorage, BrowserSessionStorageProvider>();

            await builder.Build().RunAsync();
        }
    }
}
