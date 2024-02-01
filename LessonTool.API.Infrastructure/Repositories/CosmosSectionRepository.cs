using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Exceptions;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Repositories;

public class CosmosSectionRepository : CosmosRepositoryBase, ISectionRepository
{
    private readonly ICosmosContainerFactory _containerFactory;


    public CosmosSectionRepository(ICosmosContainerFactory cosmosContainerFactory)
    {
        _containerFactory = cosmosContainerFactory;
    }

    public async Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM Data WHERE Data.{nameof(CosmosSection.Type)} = '{CosmosConstants.SectionTypeName}' AND Data.{nameof(CosmosSection.LessonId)} = {lessonId}";

        var items = await ReadCosmosIterator<CosmosSection>(_containerFactory, query, cancellationToken);
        return items
            .Select(x => x.ToSectionDto())
            .ToList();
    }

    public async Task<SectionDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .ReadItemAsync<CosmosSection>(id.ToString(), new(CosmosConstants.SectionTypeName), cancellationToken: cancellationToken);

        if (!cosmosResponse.IsSuccess())
            throw new CosmosActionException("Cannot get lesson from cosmos because the response did not indicate success!", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToSectionDto();
    }

    public async Task<SectionDto> CreateAsync(SectionDto section, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(section);

        if (section.Id.IsNotEmpty())
            throw new ArgumentNullException(nameof(section), "Cannot create a new lesson because the provided Dto already has an Id.");

        //Build a valid Cosmos entity
        var cosmosSection = section.ToCosmosSection();
        cosmosSection.Id = Guid.NewGuid();

        //Save to database
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .CreateItemAsync(cosmosSection, new(CosmosConstants.SectionTypeName), cancellationToken: cancellationToken);

        //If the resource wasn't created, throw an exception
        if (!cosmosResponse.IsCreated())
            throw new CosmosActionException($"Failed to create section [{section.Id}]", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToSectionDto();
    }

    public async Task<SectionDto> UpdateAsync(SectionDto section, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(section);

        if (section.Id.IsEmpty())
            throw new ArgumentNullException(nameof(section), "Cannot create a new lesson because the provided Dto already has an Id.");

        //Build a valid Cosmos entity
        var cosmosSection = section.ToCosmosSection();

        //Save to database
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .ReplaceItemAsync(cosmosSection, cosmosSection.Id.ToString(), new(CosmosConstants.SectionTypeName));

        //If the resource wasn't created, throw an exception
        if (!cosmosResponse.IsCreated())
            throw new CosmosActionException($"Failed to create lesson [{section.Id}]", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToSectionDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _containerFactory
            .CreateDataContainer()
            .DeleteItemAsync<CosmosSection>(id.ToString(), new(CosmosConstants.SectionTypeName), cancellationToken: cancellationToken);
    }
}
