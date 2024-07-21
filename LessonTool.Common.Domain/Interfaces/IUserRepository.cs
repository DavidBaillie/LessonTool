using LessonTool.Common.Domain.Models;

namespace LessonTool.Common.Domain.Interfaces;

public interface IUserRepository : IRepository<UserDto>
{
    Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
