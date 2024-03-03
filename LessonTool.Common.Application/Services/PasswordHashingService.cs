using LessonTool.Common.Application.Interfaces;

namespace LessonTool.Common.Application.Services;

public class PasswordHashingService : IStringHashService
{
    public string HashPassword(string password, string? salt = null)
    {
        return string.Empty;
    }
}
