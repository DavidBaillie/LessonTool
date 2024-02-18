using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonPage
{
    [Inject]
    private IRepository<LessonDto> lessonRepository {  get; set; }  

    [Inject]
    private ISectionRepository sectionRepository { get; set; }

    private LessonAccessorComposite lessonAccessor = null;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!navigationManager.TryGetQueryString("Id", out Guid lessonId, new(Guid.TryParse)))
            return;

        var lessonLoadTask = lessonRepository.GetAsync(lessonId, cancellationToken);
        var sectionsLoadTask = sectionRepository.GetSectionsByLessonAsync(lessonId, cancellationToken);

        await Task.WhenAll(sectionsLoadTask, lessonLoadTask);

        lessonAccessor = new(lessonLoadTask.Result, sectionsLoadTask.Result);
    }

    private class LessonAccessorComposite : ISectionRepository
    {
        public LessonDto Lesson { get; set; }

        public LessonAccessorComposite(LessonDto lesson, List<SectionDto> sections)
        {
            Lesson = lesson;
            Lesson.Sections = sections;
        }


        public Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Lesson.Sections);
        }


        public Task<SectionDto> CreateAsync(SectionDto entity, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
        public Task<SectionDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
        public Task<SectionDto> UpdateAsync(SectionDto entity, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
    }
}