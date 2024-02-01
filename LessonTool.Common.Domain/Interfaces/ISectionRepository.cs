using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces;

public interface ISectionRepository : IRepository<SectionDto>
{
    /// <summary>
    /// Gets a list of Section DTOs with the given lessonId
    /// </summary>
    /// <param name="lessonId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default);
}