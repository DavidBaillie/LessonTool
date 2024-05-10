namespace LessonTool.Common.Domain.Models.Authentication;

public sealed class PasswordResetRequestModel
{
    public Guid RequestToken { get; set; }
    public string ResetToken { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
