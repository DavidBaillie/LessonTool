using LessonTool.Common.Domain.Interfaces;
using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.HttpServices;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient(ApiEndpointConstants.LessonsEndpoint, options =>
            {
                options.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
            });
            builder.Services.AddScoped<ILessonRepository, LessonApiService>();

            await builder.Build().RunAsync();
        }
    }
}
