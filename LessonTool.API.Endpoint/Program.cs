using LessonTool.API.Endpoint.Controllers;
using LessonTool.API.Endpoint.Middleware;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Factories;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Options;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LessonTool.API.Endpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidIssuer = builder.Configuration.GetSection("JwtOptions")["Issuer"],
                    ValidAudience = builder.Configuration.GetSection("JwtOptions")["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtOptions")["Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.Configure<CosmosClientOption>(
            builder.Configuration.GetSection("CosmosOptions"));

        // Add services to the container.
        builder.Services.AddScoped<ILessonRepository, EFCosmosLessonRepository>();
        builder.Services.AddScoped<ISectionRepository, CosmosSectionRepository>();
        builder.Services.AddScoped<ICosmosContainerFactory, CosmosContainerFactory>();

        builder.Services.AddDbContext<CosmosDbContext>();

        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(policy => {

            policy.AddPolicy("CORS_Policy", builder =>
              builder.WithOrigins("*")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyOrigin()
                .AllowAnyMethod());
        });


        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            
        }

        app.UseCors("CORS_Policy");
        app.UseHttpsRedirection();

        //TODO - Enable this later when everything is working
        //app.UseMiddleware<InternalServerErrorMiddleware>();
        //app.UseMiddleware<DataAccessErrorMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
