using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Options;

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
        builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
        builder.Services.AddServices();

        builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //builder.Services.AddCustomCorsPolicy();
        builder.Services.AddCors(policy => {

            policy.AddPolicy("CORS_Policy", builder =>
              builder.WithOrigins("*")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
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

#if DEBUG
        //Setup for the in memory option
        if (builder.Configuration["UseInMemory"] == "true")
        {
            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<SqlLiteConnectionPersistor>();
                scope.ServiceProvider.GetRequiredService<SqlLiteDatabaseInitializer>();
            }
        }
#endif

        app.Run();
    }
}
