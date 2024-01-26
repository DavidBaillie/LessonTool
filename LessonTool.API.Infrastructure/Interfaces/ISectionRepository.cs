using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces
{
    public interface ISectionRepository
    {
        Task<SectionDto> CreateSectionAsync(SectionDto lesson, CancellationToken cancellationToken = default);
        Task DeleteSectionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SectionDto> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<SectionDto>> GetSectionsByLesson(Guid lessonId, CancellationToken cancellationToken = default);
        Task<SectionDto> UpdateSectionAsync(SectionDto lesson, CancellationToken cancellationToken = default);
    }
}