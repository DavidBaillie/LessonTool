using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class LessonHeader
{
    [Parameter, EditorRequired]
    public LessonDto Lesson { get; set; }
}