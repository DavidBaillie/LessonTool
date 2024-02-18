using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonPage
{
    [Inject]
    private IRepository<LessonDto> lessonRepository {  get; set; }  

    [Inject]
    private ISectionRepository sectionRepository { get; set; }

    [Parameter]
    public Guid Id { get; set; }

    private LessonDto displayLesson = new();


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Id == Guid.Empty)
        {
            displayLesson = null;
            return;
        }

        var lessonLoadTask = lessonRepository.GetAsync(Id, cancellationToken);
        var sectionsLoadTask = sectionRepository.GetSectionsByLessonAsync(Id, cancellationToken);

        await Task.WhenAll(sectionsLoadTask, lessonLoadTask);

        displayLesson = lessonLoadTask.Result;
        displayLesson.Sections = sectionsLoadTask.Result;
    }
}
