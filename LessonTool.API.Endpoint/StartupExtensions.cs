using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Services;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Services;
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
        services.AddScoped<IUserAccountRepository, EFCosmosUserAccountRepository>();
        services.AddScoped<ILoginSessionRepository, EFCosmosLoginSessionRepository>();
        
        services.AddTransient<IHashService, HashService>();
        services.AddTransient<ITokenGenerationService, TokenGenerationService>();
        services.AddTransient<ILoginRequestProcessor, LoginRequestProcessor>();
        services.AddTransient<IPasswordComplexityValidator, PasswordComplexityValidator>();
    }


    public static void AddAuthenticationAndAuthorization(this IServiceCollection services)
    {
        //services.AddAuthentication();
        services.AddAuthorization();
    }

    public static void AddCustomCorsPolicy(this IServiceCollection services)
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
