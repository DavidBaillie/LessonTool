namespace LessonTool.Common.Domain.Models;

public class LessonDto : EntityDtoBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime VisibleDate { get; set; }
    public List<SectionDto> Sections { get; set; } = new List<SectionDto>();
}
