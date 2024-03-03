using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.API.Infrastructure.Repositories;

public sealed class CosmosUserRepository : CosmosRepositoryBase, IRepository<CosmosUser>
{
    public Task<CosmosUser> CreateAsync(CosmosUser entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CosmosUser> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CosmosUser> UpdateAsync(CosmosUser entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
