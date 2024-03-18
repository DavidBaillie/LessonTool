using LessonTool.API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.EntityFramework;

public class CosmosDbContext : DbContext
{
    public DbSet<CosmosLesson> Lessons { get; set; }
    public DbSet<CosmosSection> Sections { get; set; }
    public DbSet<CosmosUserAccount> UserAccounts { get; set; }
    public DbSet<CosmosLoginSession> LoginSessions { get; set; }

    public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CosmosLesson>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        modelBuilder.Entity<CosmosSection>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        modelBuilder.Entity<CosmosUserAccount>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        modelBuilder.Entity<CosmosLoginSession>()
            .ToContainer("Data")
            .HasPartitionKey("Type");

        base.OnModelCreating(modelBuilder);
    }
}