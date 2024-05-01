using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using LessonTool.UI.WebApp.Extensions;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonsPage
{
    [Inject]
    private ILessonRepository lessonRepository { get; set; }

    [Inject]
    private IAuthenticationStateHandler authenticationStateHandler { get; set; }

    private List<LessonDto> lessons = new();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            lessons = await lessonRepository.GetAllInDateRangeAsync(cancellationToken: cancellationToken);

            var username = (await authenticationStateHandler.GetSecurityTokenAsync(cancellationToken)).GetUsernameClaim();
            if (username == "Anonymous")
                lessons = lessons.Where(x => x.VisibleDate < DateTime.UtcNow).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load lessons: {ex}");
        }
    }
}
