namespace LessonTool.API.Infrastructure.Models;

public class CosmosSection : CosmosEntityBase
{
    public string LessonId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
}
