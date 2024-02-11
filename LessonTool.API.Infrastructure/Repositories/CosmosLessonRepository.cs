using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Exceptions;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Exceptions;
using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.Azure.Cosmos;

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
        try
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
        catch (CosmosException ex)
        {
            throw new DataAccessException($"Failed to find all lessons between Min({min}) and Max({max}) because a cosmos exception was encountered!", ex);
        }
    }

    public async Task<LessonDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cosmosResponse = await _containerFactory
            .CreateDataContainer()
            .ReadItemAsync<CosmosLesson>(id.ToString(), new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);

            if (!cosmosResponse.IsSuccess())
                throw new CosmosActionException("Cannot get lesson from cosmos because the response did not indicate success!", cosmosResponse.StatusCode);

            return cosmosResponse.Resource.ToLessonDto();
        }
        catch (CosmosException ex)
        {
            throw new DataAccessException($"Failed to get the entity [{id}] because a cosmos exception was encountered!", ex);
        }
    }

    public async Task<LessonDto> CreateAsync(LessonDto lesson, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(lesson);

            if (lesson.Id.IsNotEmpty())
                throw new ArgumentNullException(nameof(lesson), "Cannot create a new lesson because the provided Dto already has an Id.");

            //Build a valid Cosmos entity
            var cosmosLesson = lesson.ToCosmosLesson();
            cosmosLesson.Id = Guid.NewGuid();
            cosmosLesson.CreatedDate = DateTime.UtcNow;

            //Save to database
            var cosmosResponse = await _containerFactory
                .CreateDataContainer()
                .CreateItemAsync(cosmosLesson, new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);

            //If the resource wasn't created, throw an exception
            if (!cosmosResponse.IsCreated())
                throw new CosmosActionException($"Failed to create lesson [{lesson.Id}] - Status Code [{cosmosResponse.StatusCode}]", cosmosResponse.StatusCode);

            return cosmosResponse.Resource.ToLessonDto();
        }
        catch (CosmosException ex)
        {
            throw new DataAccessException($"Failed to create a new lesson because a cosmos exception was encountered!", ex);
        }
    }


    public async Task<LessonDto> UpdateAsync(LessonDto lesson, CancellationToken cancellationToken = default)
    {
        try
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
            if (!cosmosResponse.IsSuccess())
                throw new CosmosActionException($"Failed to create lesson [{lesson.Id}] - [{cosmosResponse.StatusCode}]", cosmosResponse.StatusCode);

            return cosmosResponse.Resource.ToLessonDto();
        }
        catch (CosmosException ex)
        {
            throw new DataAccessException($"Failed to update the lesson entity [{lesson.Id}] because a cosmos exception was encountered!", ex);
        }
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _containerFactory
            .CreateDataContainer()
            .DeleteItemAsync<CosmosLesson>(id.ToString(), new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);
        }
        catch (CosmosException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                return;

            throw new DataAccessException($"Failed to delete lesson entity [{id}] because a COSMOS exception was encountered!", ex);
        }
    }
}
