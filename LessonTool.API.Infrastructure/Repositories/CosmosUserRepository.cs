using Lessontool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.API.Infrastructure.Repositories;

public sealed class CosmosUserRepository : CosmosRepositoryBase, IRepository<AuthenticatedUser>
{
    public Task<AuthenticatedUser> CreateAsync(AuthenticatedUser entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticatedUser> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticatedUser> UpdateAsync(AuthenticatedUser entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
