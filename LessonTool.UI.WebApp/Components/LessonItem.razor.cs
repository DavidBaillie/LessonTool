using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class LessonItem
{
    [Parameter]
    public LessonDto Lesson { get; set; }

    private string lessonName { get => Lesson is null ? "N/A" : Lesson.Name; }
    private string lessonDescription { get => Lesson is null ?  "N/A" : Lesson.Description; }
}
