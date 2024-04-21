namespace LessonTool.API.Infrastructure.EntityFramework;

public class SqlLiteDatabaseInitializer
{
    public SqlLiteDatabaseInitializer(CosmosDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
