using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using LessonTool.UI.WebApp.Extensions;
using LessonTool.Common.Domain.Constants;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonsPage
{
    [Inject]
    private ILessonRepository lessonRepository { get; set; }

    [Inject]
    private IAuthenticationStateHandler authenticationStateHandler { get; set; }

    private List<LessonDto> lessons = new();
    private bool isManagingUser = false;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            lessons = await lessonRepository.GetAllInDateRangeAsync(cancellationToken: cancellationToken);

            isManagingUser = (await authenticationStateHandler.GetSecurityTokenAsync(cancellationToken))
                .TokenHasAnyClaims([UserClaimConstants.Admin, UserClaimConstants.Teacher]);
            
            if (!isManagingUser)
                lessons = lessons.Where(x => x.VisibleDate < DateTime.UtcNow).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load lessons: {ex}");
        }
    }
}
