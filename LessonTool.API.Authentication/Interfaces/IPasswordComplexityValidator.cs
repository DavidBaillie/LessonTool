namespace LessonTool.API.Authentication.Interfaces
{
    public interface IPasswordComplexityValidator
    {
        bool PasswordIsSufficient(string password);
    }
}