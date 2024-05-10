namespace LessonTool.Common.Domain.Models.Authentication;

public sealed class RefreshTokensRequestModel
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public Guid RequestToken { get; set; }
}
