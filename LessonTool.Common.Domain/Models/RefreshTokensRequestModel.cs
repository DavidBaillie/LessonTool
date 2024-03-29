namespace LessonTool.Common.Domain.Models;

public class RefreshTokensRequestModel
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public Guid RequestToken { get; set; }
}
