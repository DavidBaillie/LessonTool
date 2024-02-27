using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.UI.Application.Interfaces
{
    public interface IFullLessonRepository : ILessonRepository
    {
        Task<LessonDto> LoadFullLessonFromSourceAsync(Guid lessonId, CancellationToken cancellationToken = default);
    }
}