using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.API.Infrastructure.Interfaces;

public interface IUserAccountRepository : IRepository<UserAccount>
{
    Task<UserAccount> GetAccountByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<List<UserAccount>> GetAllAsync(CancellationToken cancellationToken = default);
}
