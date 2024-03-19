namespace LessonTool.Common.Domain.Models;

public class LoginRequestModel
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public Guid RequestToken { get; set; }
}
