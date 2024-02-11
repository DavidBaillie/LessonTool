using LessonTool.Common.Domain.Models;

namespace LessonTool.Common.Domain.Interfaces;

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