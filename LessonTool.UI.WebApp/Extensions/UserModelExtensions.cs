using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.FormModels;

namespace LessonTool.UI.WebApp.Extensions;

public static class UserModelExtensions
{
    public static UserFormModel ToFormModel(this UserDto user)
    {
        return new UserFormModel()
        {
            Id = user.Id,
            Username = user.Username,
            UserRole = user.AccountType
        };
    }

    public static UserDto ToDto(this UserFormModel model)
    {
        return new UserDto()
        {
            Id = model.Id,
            Username = model.Username,
            AccountType = model.UserRole
        };
    }
}
