using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class LessonItem
{
    [Inject]
    private LessonDto lesson { get; set; }

    private string lessonName { get => lesson is null ? "N/A" : lesson.Name; }
    private string lessonDescription { get => lesson is null ?  "N/A" : lesson.Description; }
}
