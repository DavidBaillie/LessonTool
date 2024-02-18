using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
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

            builder.Services.AddHttpClient(ApiEndpointConstants.CommonApiClientName, options =>
            {
                options.BaseAddress = new Uri("https://localhost:44360");
            });

            builder.Services.AddScoped<IRepository<LessonDto>, MockLessonApiService>();

            builder.Services.AddScoped<ILessonRepository, MockLessonApiService>();
            builder.Services.AddScoped<ISectionRepository, MockSectionApiService>();

            await builder.Build().RunAsync();
        }
    }
}
