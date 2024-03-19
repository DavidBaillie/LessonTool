namespace LessonTool.Common.Domain.Interfaces
{
    public interface IHashService
    {
        byte[] CreateSalt();
        string HashString(string content);
        string HashStringAndSalt(string password, byte[] salt);
    }
}