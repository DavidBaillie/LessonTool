namespace LessonTool.Common.Domain.Models;

public class SectionDto : EntityDtoBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
}
