namespace LessonTool.API.Infrastructure.Models;

public class CosmosLoginSession : CosmosEntityBase
{
    public string UserAccountId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresDateTime { get; set; }
}