using LessonTool.API.Infrastructure.Factories;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Options;
using LessonTool.API.Infrastructure.Repositories;

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

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
