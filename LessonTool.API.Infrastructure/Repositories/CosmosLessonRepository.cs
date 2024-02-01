using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Exceptions;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Repositories;

public class CosmosLessonRepository : CosmosRepositoryBase, ILessonRepository
{
    private readonly ICosmosContainerFactory _containerFactory;


    public CosmosLessonRepository(ICosmosContainerFactory cosmosContainerFactory)
    {
        _containerFactory = cosmosContainerFactory;
    }


    public async Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
    {
        var query = $"SELECT * FROM Data WHERE Data.Type = '{CosmosConstants.LessonTypeName}'";

        if (min.HasValue)
            query += $" AND {nameof(CosmosLesson.VisibleDate)} > {min.Value}";

        if (max.HasValue)
            query += $" AND {nameof(CosmosLesson.VisibleDate)} < {max.Value}";

        var items = await ReadCosmosIterator<CosmosLesson>(_containerFactory, query, cancellationToken);
        return items
            .Select(x => x.ToLessonDto())
            .ToList();
    }

    public async Task<LessonDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .ReadItemAsync<CosmosLesson>(id.ToString(), new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);

        if (!cosmosResponse.IsSuccess())
            throw new CosmosActionException("Cannot get lesson from cosmos because the response did not indicate success!", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToLessonDto();
    }

    public async Task<LessonDto> CreateAsync(LessonDto lesson, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (lesson.Id.IsNotEmpty())
            throw new ArgumentNullException(nameof(lesson), "Cannot create a new lesson because the provided Dto already has an Id.");

        //Build a valid Cosmos entity
        var cosmosLesson = lesson.ToCosmosLesson();
        cosmosLesson.Id = Guid.NewGuid();

        //Save to database
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .CreateItemAsync(cosmosLesson, new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);

        //If the resource wasn't created, throw an exception
        if (!cosmosResponse.IsCreated())
            throw new CosmosActionException($"Failed to create lesson [{lesson.Id}]", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToLessonDto();
    }


    public async Task<LessonDto> UpdateAsync(LessonDto lesson, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (lesson.Id.IsEmpty())
            throw new ArgumentNullException(nameof(lesson), "Cannot create a new lesson because the provided Dto already has an Id.");

        //Build a valid Cosmos entity
        var cosmosLesson = lesson.ToCosmosLesson();

        //Save to database
        var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .ReplaceItemAsync(cosmosLesson, cosmosLesson.Id.ToString(), new(CosmosConstants.LessonTypeName));

        //If the resource wasn't created, throw an exception
        if (!cosmosResponse.IsCreated())
            throw new CosmosActionException($"Failed to create lesson [{lesson.Id}]", cosmosResponse.StatusCode);

        return cosmosResponse.Resource.ToLessonDto();
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _containerFactory
            .CreateDataContainer()
            .DeleteItemAsync<CosmosLesson>(id.ToString(), new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);
    }
}
