using LessonTool.API.Infrastructure.Exceptions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Models;
using Microsoft.Azure.Cosmos;

namespace LessonTool.API.Infrastructure.Repositories
{
    public class CosmosLessonRepository : ILessonRepository
    {
        private readonly ICosmosContainerFactory _containerFactory;


        public CosmosLessonRepository(ICosmosContainerFactory cosmosContainerFactory)
        {
            _containerFactory = cosmosContainerFactory;
        }


        public async Task<LessonDto> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cosmosResponse = await _containerFactory
                .CreateDataContainer()
                .ReadItemAsync<CosmosLesson>(id.ToString(), new(CosmosConstants.LessonTypeName), cancellationToken: cancellationToken);

            if (!cosmosResponse.IsSuccess())
                throw new CosmosActionException("Cannot get lesson from cosmos because the response did not indicate success!", cosmosResponse.StatusCode);

            return cosmosResponse.Resource.ToLessonDto();
        }


        public async Task<List<LessonDto>> GetLessonsAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
        {
            var query = $"SELECT * FROM Data WHERE Data.Type = '{CosmosConstants.LessonTypeName}'";

            if (min.HasValue)
                query += $" AND {nameof(CosmosLesson.VisibleDate)} > {min.Value}";

            if (max.HasValue)
                query += $" AND {nameof(CosmosLesson.VisibleDate)} < {max.Value}";

            var feedIterator = _containerFactory
                .CreateDataContainer()
                .GetItemQueryIterator<CosmosLesson>(new QueryDefinition(query));
            
            var lessons = new List<CosmosLesson>();
            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);
                lessons.AddRange(response.Resource);
            }

            return lessons
                .Select(x => x.ToLessonDto())
                .ToList();
        }


        public async Task<LessonDto> CreateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default)
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


        public async Task<LessonDto> UpdateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task DeleteLessonAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
