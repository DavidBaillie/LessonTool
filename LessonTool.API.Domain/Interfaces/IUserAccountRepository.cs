using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.API.Domain.Interfaces;

public interface ILoginSessionRepository : IRepository<UserLoginSession>
{
    Task DeleteExpiredSessions();
}
