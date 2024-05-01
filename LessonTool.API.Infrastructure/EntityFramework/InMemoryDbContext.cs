using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Constants;
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
                },
                new CosmosUserAccount()
                {
                    Id = "353a7311-8c13-4dad-860b-d6f1556088ec",
                    Username = "Teacher",
                    AccountType = UserClaimConstants.Teacher,
                    Password = "978F1840D3FD375BBF43A4ECE1C11458C8EA7214D4452BE2E8BEC3FD1142FBBEA568A2671FF87D314F6FCAD21068397DED1FD08A8AC7057920AAEF0874C3A321", // Teacher
                    PasswordSalt = "NbjsvmoVl6XBe1llRrQbyrJEFGANGSfedaL2RfyV6akFxJ4U26IWPjKkOT/PV0cbxfqVTjRlYoKLXIuF1aFc+g==",
                    PasswordResetToken = string.Empty
                },
                new CosmosUserAccount()
                {
                    Id = "5adcfee4-c555-4118-ae3d-4650f079a08c",
                    Username = "Student",
                    AccountType = UserClaimConstants.Student,
                    Password = "3F94E945E0E3FECF4C6AC289E149DB6AFAE1D5DD876EF912D6D44FCDB22C21BE86329A4E4852666DF4AFF2C951E14AB09168492EEA728E926A2AFEE9BE13FE52", // Student
                    PasswordSalt = "zX/Tt8xSIuQ/D8YDngTbES16Tx2JFPa9qcTlUakv4afU3lBEgy6TrZa0AXiKjRLjUYsD+sZIQ8XRxMqrME2gdw==",
                    PasswordResetToken = string.Empty
                });

        modelBuilder.Entity<CosmosLesson>()
            .HasData(
            new CosmosLesson()
            {
                Id = "41ba1eef-2773-482a-8129-e51ce5531c69",
                Name = "Lorem ipsum dolor sit",
                Description = "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa",
                VisibleDate = DateTime.MinValue.AddDays(1),
                PlannedDate = DateTime.UtcNow.AddDays(-1),
                Type = "Lesson"
            },
            new CosmosLesson()
            {
                Id = "7023ece4-547d-4c44-95f1-1cc26782a085",
                Name = "Quis nostrud exercitation ullamco",
                Description = "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa",
                VisibleDate = DateTime.UtcNow.AddDays(1),
                PlannedDate = DateTime.UtcNow.AddDays(2),
                Type = "Lesson"
            });

        modelBuilder.Entity<CosmosSection>()
            .HasData(
            new CosmosSection()
            {
                Id = "a3f151d3-dd7a-4d9d-8ac2-cac919b02ef1",
                LessonId = "41ba1eef-2773-482a-8129-e51ce5531c69",
                Title = "Excepteur sint occaecat",
                Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
                CreatedDate = DateTime.MinValue.AddDays(1),
                Type = "Section"
            },
            new CosmosSection()
            {
                Id = "96427393-76b5-48c2-ac3d-f8aec785a497",
                LessonId = "41ba1eef-2773-482a-8129-e51ce5531c69",
                Title = "Excepteur sint occaecat",
                Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
                CreatedDate = DateTime.MinValue.AddDays(1),
                Type = "Section"
            },
            new CosmosSection()
            {
                Id = "63512242-aeaa-493e-a4ef-ba83afc7e51f",
                LessonId = "7023ece4-547d-4c44-95f1-1cc26782a085",
                Title = "Excepteur sint occaecat",
                Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
                CreatedDate = DateTime.MinValue.AddDays(1),
                Type = "Section"
            });
    }
}
