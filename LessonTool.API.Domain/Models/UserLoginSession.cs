namespace LessonTool.API.Authentication.Models;

public class UserLoginSession
{
    public Guid Id { get; set; }
    public Guid UserAccountId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresDateTime { get; set; }
}
