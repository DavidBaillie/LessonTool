using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Section;

public partial class SectionList
{
    [Parameter, EditorRequired]
    public List<SectionDto> Sections { get; set; } = new();
}
