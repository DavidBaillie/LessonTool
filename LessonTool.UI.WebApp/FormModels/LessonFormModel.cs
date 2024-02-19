using System.ComponentModel.DataAnnotations;

namespace LessonTool.UI.WebApp.FormModels;

public class LessonFormModel
{
    public Guid Id { get; set; }

    [MinLength(1, ErrorMessage = "Lessons must contain a Title of some form")]
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime VisibleDate { get; set; }

    public DateTime PlannedDate { get; set; }

    public List<SectionFormModel> Sections { get; set; } = new();
}