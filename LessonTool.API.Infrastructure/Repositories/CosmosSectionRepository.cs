using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Repositories;

public class CosmosSectionRepository : ISectionRepository
{
    private readonly ICosmosContainerFactory _containerFactory;


    public CosmosSectionRepository(ICosmosContainerFactory cosmosContainerFactory)
    {
        _containerFactory = cosmosContainerFactory;
    }

    public async Task<List<SectionDto>> GetSectionsByLessonAsync(Guid sectionId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SectionDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SectionDto> CreateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SectionDto> UpdateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
