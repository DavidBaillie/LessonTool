using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonEditor
{
    [Inject]
    public ILessonRepository LessonRepository { get; set; }

    [Inject]
    public ISectionRepository SectionRepository { get; set; }

    private LessonDto lesson;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!navigationManager.TryGetQueryString("Id", out Guid lessonId, new(Guid.TryParse)))
            return;

        var lessonLoadTask = LessonRepository.GetAsync(lessonId, cancellationToken);
        var sectionsLoadTask = SectionRepository.GetSectionsByLessonAsync(lessonId, cancellationToken);

        await Task.WhenAll(sectionsLoadTask, lessonLoadTask);

        lesson = lessonLoadTask.Result;
        lesson.Sections = sectionsLoadTask.Result;
    }
}