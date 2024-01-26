using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces
{
    public interface ILessonRepository
    {
        Task<LessonDto> CreateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default);
        Task DeleteLessonAsync(Guid id, CancellationToken cancellationToken = default);
        Task<LessonDto> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<LessonDto>> GetLessonsAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default);
        Task<LessonDto> UpdateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default);
    }
}