namespace LessonTool.Common.Domain.Models.Authentication;

public sealed class AccessTokensResponseModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}
