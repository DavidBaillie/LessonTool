using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonPage
{
    [Inject]
    private ILessonRepository lessonRepository {  get; set; }  

    [Inject]
    private ISectionRepository sectionRepository { get; set; }

    private LessonDto lesson = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!navigationManager.TryGetQueryString("Id", out Guid lessonId, new(Guid.TryParse)))
            return;

        var lessonLoadTask = lessonRepository.GetAsync(lessonId, cancellationToken);
        var sectionsLoadTask = sectionRepository.GetSectionsByLessonAsync(lessonId, cancellationToken);

        await Task.WhenAll(sectionsLoadTask, lessonLoadTask);

        lesson = lessonLoadTask.Result;
        lesson.Sections = sectionsLoadTask.Result;
    }
}