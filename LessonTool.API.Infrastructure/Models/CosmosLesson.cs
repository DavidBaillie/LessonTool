namespace LessonTool.API.Infrastructure.Models;

public class CosmosLesson : CosmosEntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PlannedDate { get; set; }
    public DateTime VisibleDate { get; set; }
}
