using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Application.Interfaces;

namespace LessonTool.UI.Application.Repositories;

public class FullLessonRepository : IFullLessonRepository
{
    private readonly ILessonRepository lessonRepository;
    private readonly ISectionRepository sectionRepository;

    public FullLessonRepository(ILessonRepository lessonRepository, ISectionRepository sectionRepository)
    {
        this.lessonRepository = lessonRepository;
        this.sectionRepository = sectionRepository;
    }

    public Task<LessonDto> CreateAsync(LessonDto entity, CancellationToken cancellationToken = default)
        => lessonRepository.CreateAsync(entity, cancellationToken);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => lessonRepository.DeleteAsync(id, cancellationToken);

    public Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
        => lessonRepository.GetAllInDateRangeAsync(min, max, cancellationToken);

    public Task<LessonDto> UpdateAsync(LessonDto entity, CancellationToken cancellationToken = default)
        => lessonRepository.UpdateAsync(entity, cancellationToken);

    public Task<LessonDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => LoadFullLessonFromSourceAsync(id, cancellationToken);

    public async Task<LessonDto> LoadFullLessonFromSourceAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var loadLessonTask = lessonRepository.GetAsync(lessonId, cancellationToken);
        var loadSectionsTask = sectionRepository.GetSectionsByLessonAsync(lessonId, cancellationToken);

        await Task.WhenAll(loadLessonTask, loadSectionsTask);

        var lesson = loadLessonTask.Result;
        lesson.Sections = loadSectionsTask.Result;

        return lesson;
    }
}
