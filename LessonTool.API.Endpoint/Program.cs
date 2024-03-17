using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Models;
using LessonTool.API.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Configuration;

namespace LessonTool.API.Endpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<CosmosClientOption>(
            builder.Configuration.GetSection("CosmosOptions"));

        // Add services to the container.
        builder.Services.AddCosmosDbContext(builder.Configuration);
        builder.Services.AddAuthenticationAndAuthorization();
        builder.Services.AddServices();

        builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCustomCorsPolicy();

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
