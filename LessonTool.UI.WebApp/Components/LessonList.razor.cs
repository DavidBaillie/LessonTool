using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class LessonList
{
    [Parameter]
    public ILessonRepository LessonRepisitory { get; set; }
    
    private List<LessonDto> lessons = new();


    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            lessons = await LessonRepisitory.GetAllInDateRangeAsync(null, null, cancellationToken);
        }
        catch (Exception ex)
        {
            
        }
    }
}
