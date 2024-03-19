namespace LessonTool.Common.Domain.Interfaces
{
    public interface IHashService
    {
        byte[] CreateSalt();
        string HashString(string content);
        string HashStringWithSalt(string password, byte[] salt);
    }
}