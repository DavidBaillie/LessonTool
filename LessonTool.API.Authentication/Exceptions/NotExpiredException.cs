namespace LessonTool.API.Authentication.Exceptions;

public class NotExpiredException : Exception
{
    public NotExpiredException() : base() { }
    public NotExpiredException(string message) : base(message) { }
    public NotExpiredException(string message, Exception inner) : base(message, inner) { }
}
