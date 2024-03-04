using LessonTool.API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LessonTool.API.Infrastructure.EntityFramework;

public class CosmosDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<CosmosLesson> Lessons { get; set; }
    public DbSet<CosmosSection> Sections { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(
            configuration.GetSection("CosmosOptions")["Endpoint"],
            configuration.GetSection("CosmosOptions")["AccountKey"],
            configuration.GetSection("CosmosOptions")["DatabaseName"]);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CosmosLesson>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        modelBuilder.Entity<CosmosSection>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        base.OnModelCreating(modelBuilder);
    }
}