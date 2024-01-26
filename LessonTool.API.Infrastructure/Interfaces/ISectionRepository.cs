using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces
{
    public interface ISectionRepository
    {
        Task<LessonDto> CreateSectionAsync(LessonDto lesson, CancellationToken cancellationToken = default);
        Task DeleteSectionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SectionDto> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<LessonDto>> GetSectionsByLesson(Guid lessonId, CancellationToken cancellationToken = default);
        Task<LessonDto> UpdateSectionAsync(LessonDto lesson, CancellationToken cancellationToken = default);
    }
}