using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Models;
using Microsoft.Azure.Cosmos;
using System;

namespace LessonTool.API.Infrastructure.Repositories
{
    public class LessonRepository
    {
        private readonly ICosmosContainerFactory _containerFactory;


        public LessonRepository(ICosmosContainerFactory cosmosContainerFactory)
        {
            _containerFactory = cosmosContainerFactory;
        }


        public async Task<List<Lesson>> GetLessonsAsync(CancellationToken cancellationToken = default)
        {
            var container = _containerFactory.CreateDataContainer();
            var lessons = new List<Lesson>();

            var queryDefinition = new QueryDefinition($"SELECT * FROM Data WHERE Data.Type = '{CosmosConstants.LessonTypeName}'");
            var feedIterator = container.GetItemQueryIterator<Lesson>(queryDefinition);

            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);
                lessons.AddRange(response.Resource);
            }

            return lessons;
        }
    }
}
