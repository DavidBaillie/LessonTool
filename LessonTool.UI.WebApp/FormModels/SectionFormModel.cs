using System.ComponentModel.DataAnnotations;

namespace LessonTool.UI.WebApp.FormModels;

public class SectionFormModel
{
    public Guid Id { get; set; }
    
    public Guid LessonId { get; set; }
    
    public string Title { get; set; }

    [MinLength(1, ErrorMessage = "Sections must contain content of some form")]
    public string Content { get; set; }
    
    public DateTime CreatedDate { get; set; }
}
