using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Section;

public partial class SectionItem
{
    [Parameter, EditorRequired]
    public SectionDto Section { get; set; }

    [Parameter]
    public int SectionIndex { get; set; } = -1;
}
