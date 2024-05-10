using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Lesson;

public partial class LessonList
{
    [Parameter, EditorRequired]
    public List<LessonDto> Lessons { get; set; } = new();

    [Parameter, EditorRequired]
    public bool IsAllowedToEdit { get; set; }
}
