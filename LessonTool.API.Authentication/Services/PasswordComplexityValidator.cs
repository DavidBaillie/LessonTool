using LessonTool.API.Authentication.Interfaces;

namespace LessonTool.API.Authentication.Services;

public class PasswordComplexityValidator : IPasswordComplexityValidator
{
    public bool PasswordIsSufficient(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Length > 11;
    }
}
