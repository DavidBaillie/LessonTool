using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Section;

public partial class SectionList
{
    [Parameter, EditorRequired]
    public Func<Task<List<SectionDto>>> SectionsSource { get; set; }

    private List<SectionDto> sections = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            sections = await SectionsSource.Invoke();
        }
        catch (Exception ex)
        {

        }
    }
}
