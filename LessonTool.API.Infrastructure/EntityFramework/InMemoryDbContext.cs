using LessonTool.API.Authentication.Constants;
using LessonTool.API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.EntityFramework;

public class InMemoryDbContext : CosmosDbContext
{
    public InMemoryDbContext(DbContextOptions<CosmosDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CosmosUserAccount>()
            .HasData(
                new CosmosUserAccount()
                {
                    Id = "",
                    Username = "Admin",
                    AccountType = UserClaimConstants.Admin,
                    Password = "",
                    PasswordSalt = "",
                    PasswordResetToken = string.Empty
                });
    }
}
