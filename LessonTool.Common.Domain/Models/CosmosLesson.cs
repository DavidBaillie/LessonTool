using LessonTool.Common.Domain.Constants;

namespace LessonTool.Common.Domain.Models;

public class CosmosLesson
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } = CosmosConstants.LessonTypeName;
    public DateTime CreatedDate { get; set; }
    public DateTime VisibleDate { get; set; }
}
