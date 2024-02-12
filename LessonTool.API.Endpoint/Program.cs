using LessonTool.API.Endpoint.Middleware;
using LessonTool.API.Infrastructure.Factories;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Options;
using LessonTool.API.Infrastructure.Repositories;
using LessonTool.Common.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace LessonTool.API.Endpoint;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<CosmosClientOption>(
            builder.Configuration.GetSection("CosmosOptions"));

        // Add services to the container.
        builder.Services.AddScoped<ILessonRepository, CosmosLessonRepository>();
        builder.Services.AddScoped<ISectionRepository, CosmosSectionRepository>();
        builder.Services.AddScoped<ICosmosContainerFactory, CosmosContainerFactory>();

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
        app.UseAuthorization();

        //TODO - Enable this later when everything is working
        //app.UseMiddleware<InternalServerErrorMiddleware>();
        //app.UseMiddleware<DataAccessErrorMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
