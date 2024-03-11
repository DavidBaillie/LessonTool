using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LessonTool.API.Endpoint;

public static class StartupExtensions
{
    public static void AddCosmosDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CosmosDbContext>(options =>
        {
#if DEBUG
            if (configuration["UseInMemory"] == "true")
            {
                options.UseInMemoryDatabase("LessonToolDatabase")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
            else
            {
                options.UseCosmos(
                    configuration.GetSection("CosmosOptions")["Endpoint"],
                    configuration.GetSection("CosmosOptions")["AccountKey"],
                    configuration.GetSection("CosmosOptions")["DatabaseName"]);
            }
#else
            options.UseCosmos(
                    configuration.GetSection("CosmosOptions")["Endpoint"],
                    configuration.GetSection("CosmosOptions")["AccountKey"],
                    configuration.GetSection("CosmosOptions")["DatabaseName"]);
#endif
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILessonRepository, EFCosmosLessonRepository>();
        services.AddScoped<ISectionRepository, EfCosmosSectionRepository>();
    }


    public static void AddAuthenticationAndAuthorization(this IServiceCollection services)
    {
        services.AddDataProtection()
            .PersistKeysToDbContext<CosmosDbContext>();

        services.AddAuthentication().AddBearerToken();
        services.AddAuthorization();
    }

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(policy => {

            policy.AddPolicy("CORS_Policy", builder =>
              builder.WithOrigins("*")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyOrigin()
                .AllowAnyMethod());
        });
    }
}
