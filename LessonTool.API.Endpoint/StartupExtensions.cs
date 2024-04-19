using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Services;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Services;
using LessonTool.UI.Infrastructure.HttpServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

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
        //services.AddScoped<ILessonRepository, EFCosmosLessonRepository>();
        //services.AddScoped<ISectionRepository, EfCosmosSectionRepository>();

        //TODO - Swap back after testing
        services.AddScoped<ILessonRepository, MockLessonRepositoryService>();
        services.AddScoped<ISectionRepository, MockSectionRepositoryService>();

        services.AddScoped<IUserAccountRepository, EFCosmosUserAccountRepository>();
        services.AddScoped<ILoginSessionRepository, EFCosmosLoginSessionRepository>();
        
        services.AddTransient<IHashService, HashService>();
        services.AddTransient<ITokenGenerationService, TokenGenerationService>();
        services.AddTransient<ILoginRequestProcessor, LoginRequestProcessor>();
        services.AddTransient<IPasswordComplexityValidator, PasswordComplexityValidator>();
    }


    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = configuration.GetSection("JwtOptions")["Audience"],
                    ValidIssuer = configuration.GetSection("JwtOptions")["Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions")["Key"])),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
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
