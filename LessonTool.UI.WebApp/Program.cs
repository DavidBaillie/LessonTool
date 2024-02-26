using LessonTool.Common.Domain.Interfaces;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Application.Repositories;
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

            builder.Services.AddScoped<IFullLessonRepository, FullLessonRepository>();

            //API Callers
            //builder.Services.AddScoped<ILessonRepository, LessonApiService>();
            //builder.Services.AddScoped<ISectionRepository, SectionApiService>();

            //TEST Callers
            builder.Services.AddSingleton<ILessonRepository, MockLessonApiService>();
            builder.Services.AddSingleton<ISectionRepository, MockSectionApiService>();

            await builder.Build().RunAsync();
        }
    }
}
