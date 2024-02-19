using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class LessonList
{
    [Parameter, EditorRequired]
    public Func<Task<List<LessonDto>>> LessonDataSource { get; set; }
    
    private List<LessonDto> lessons = new();


    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            lessons = await LessonDataSource.Invoke();
        }
        catch (Exception ex)
        {
            
        }
    }
}
