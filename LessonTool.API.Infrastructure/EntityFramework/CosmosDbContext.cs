using LessonTool.API.Infrastructure.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.EntityFramework;

public class CosmosDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<CosmosLesson> Lessons { get; set; }
    public DbSet<CosmosSection> Sections { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options) { }

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