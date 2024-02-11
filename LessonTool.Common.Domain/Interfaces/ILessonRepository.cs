using LessonTool.Common.Domain.Models;

namespace LessonTool.Common.Domain.Interfaces;

public interface ILessonRepository : IRepository<LessonDto>
{
    /// <summary>
    /// Gets all the Lesson DTOs inside the given daterange
    /// </summary>
    /// <param name="min">Min DateTime</param>
    /// <param name="max">Max DateTime</param>
    /// <param name="cancellationToken">Process token</param>
    Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default);
}