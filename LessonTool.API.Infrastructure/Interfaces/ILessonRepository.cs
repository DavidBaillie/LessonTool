using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces;

public interface ILessonRepository : IRepository<LessonDto>
{
    Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default);
}