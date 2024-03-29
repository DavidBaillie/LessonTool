using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.API.Domain.Interfaces;

public interface ILoginSessionRepository : IRepository<UserLoginSession>
{
    Task<UserLoginSession> GetSessionByUserIdAsync(string userId, string refreshToken, CancellationToken cancellationToken = default);
    Task DeleteExpiredSessionsAsync();
}
