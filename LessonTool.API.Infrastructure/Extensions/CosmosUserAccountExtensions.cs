using LessonTool.API.Authentication.Models;
using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Models;

namespace LessonTool.API.Infrastructure.Extensions;

public static class CosmosUserAccountExtensions
{
    public static CosmosUserAccount ToCosmosUserAccount(this UserAccount userAccount)
    {
        return new CosmosUserAccount()
        {
            Id = userAccount.Id.ToString(),
            Type = CosmosConstants.UserAccountTypeName,
            Username = userAccount.Username,
            Password = userAccount.Password,
            PasswordSalt = userAccount.PasswordSalt,
            PasswordResetToken = userAccount.PasswordResetToken
        };
    }

    public static UserAccount ToUserAccount(this CosmosUserAccount userAccount)
    {
        return new UserAccount()
        {
            Id = new Guid(userAccount.Id.ToString()),
            Username = userAccount.Username,
            Password = userAccount.Password,
            PasswordSalt = userAccount.PasswordSalt,
            PasswordResetToken= userAccount.PasswordResetToken
        };
    }
}
