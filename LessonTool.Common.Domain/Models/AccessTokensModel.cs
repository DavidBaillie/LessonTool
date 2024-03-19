namespace LessonTool.Common.Domain.Models;

public class AccessTokensModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}
