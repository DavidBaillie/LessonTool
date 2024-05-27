using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Extensions;

public static class UserAccountExtensions
{
    public static UserDto ToUserDto(this UserAccount userAccount)
    {
        return new UserDto()
        {
            Id = userAccount.Id,
            AccountType = userAccount.AccountType,
            Username = userAccount.Username
        };
    }
}
