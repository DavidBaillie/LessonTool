using LessonTool.API.Authentication.Constants;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Services;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LessonTool.API.Endpoint;

public static class StartupExtensions
{
    public static void AddCosmosDbContext(this IServiceCollection services, IConfiguration configuration)
    {
#if DEBUG
        //If in debug and using a memory database, initialize only that
        if (configuration["UseInMemory"] == "true")
        {
            services.AddDbContext<CosmosDbContext, InMemoryDbContext>(options =>
            {
                options.UseSqlite("DataSource=file::memory:?cache=shared");
            });

            services.AddTransient<SqlLiteDatabaseInitializer>();
            services.AddSingleton<SqlLiteConnectionPersistor>();
         
            return;
        }
#endif

        //Setup standard cosmos db
        services.AddDbContext<CosmosDbContext>(options =>
        {
            options.UseCosmos(
                    configuration.GetSection("CosmosOptions")["Endpoint"],
                    configuration.GetSection("CosmosOptions")["AccountKey"],
                    configuration.GetSection("CosmosOptions")["DatabaseName"]);
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        //services.AddScoped<ILessonRepository, EFCosmosLessonRepository>();
        //services.AddScoped<ISectionRepository, EfCosmosSectionRepository>();

        services.AddHttpContextAccessor();

        //TODO - Swap back after testing
        services.AddScoped<ILessonRepository, EFCosmosLessonRepository>();
        services.AddScoped<ISectionRepository, EfCosmosSectionRepository>();

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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNameConstants.ReaderPolicy, 
                x => x.RequireRole(UserClaimConstants.Admin, UserClaimConstants.Teacher, UserClaimConstants.Parent, UserClaimConstants.Student, UserClaimConstants.Reader));

            options.AddPolicy(PolicyNameConstants.StudentPolicy,
                x => x.RequireRole(UserClaimConstants.Admin, UserClaimConstants.Teacher, UserClaimConstants.Parent, UserClaimConstants.Student));

            options.AddPolicy(PolicyNameConstants.ParentPolicy,
                x => x.RequireRole(UserClaimConstants.Admin, UserClaimConstants.Teacher, UserClaimConstants.Parent));

            options.AddPolicy(PolicyNameConstants.TeacherPolicy,
                x => x.RequireRole(UserClaimConstants.Admin, UserClaimConstants.Teacher));

            options.AddPolicy(PolicyNameConstants.AdminPolicy,
                x => x.RequireRole(UserClaimConstants.Admin));
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
