namespace LessonTool.Common.Application.Interfaces
{
    public interface IStringHashService
    {
        string HashPassword(string password, string? salt = null);
    }
}