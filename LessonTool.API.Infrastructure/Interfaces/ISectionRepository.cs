using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces;

public interface ISectionRepository : IRepository<SectionDto>
{
    Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default);
}