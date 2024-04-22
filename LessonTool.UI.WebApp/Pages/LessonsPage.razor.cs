using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonsPage
{
    [Inject]
    private ILessonRepository lessonRepository { get; set; }

    private List<LessonDto> lessons = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            lessons = await lessonRepository.GetAllInDateRangeAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load lessons: {ex}");
        }
    }
}
