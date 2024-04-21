using LessonTool.API.Authentication.Constants;
using LessonTool.API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.EntityFramework;

public class InMemoryDbContext : CosmosDbContext
{
    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CosmosUserAccount>()
            .HasData(
                new CosmosUserAccount()
                {
                    Id = "6783e985-a994-4cd8-ba7e-5579f96ac85b",
                    Username = "Admin",
                    AccountType = UserClaimConstants.Admin,
                    Password = "4857907CCC4F49EF2FF5A3A79622BE96900D0C442282450D18D360F0FA9727C14F7D47AFB94BAEBD14356BCBD725CF9F24F5985F56EF57DF7747A7D34A3E5DF9", // Admin
                    PasswordSalt = "TTcOaanSbBnyDlrYyUiaMIroKstGY58l96ImrxENQoHndjCNyApw4RElYFFiaVkNqzd+lHnW31L4P14GB2krPQ==",
                    PasswordResetToken = string.Empty
                });
    }
}
