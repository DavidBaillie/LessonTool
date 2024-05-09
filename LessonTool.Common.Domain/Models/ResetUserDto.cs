namespace LessonTool.Common.Domain.Models;

public class ResetUserDto : UserDto
{
    public string PasswordResetToken { get; set; }
}
