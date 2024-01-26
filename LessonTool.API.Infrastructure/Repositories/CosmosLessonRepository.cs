using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Models;
using Microsoft.Azure.Cosmos;
using System;

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
            throw new NotImplementedException();
        }


        public async Task<List<LessonDto>> GetLessonsAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();

            //min ??= DateTime.MinValue;
            //max ??= DateTime.MaxValue;

            //var container = _containerFactory.CreateDataContainer();
            //var lessons = new List<CosmosLesson>();

            //var queryDefinition = new QueryDefinition($"SELECT * FROM Data WHERE Data.Type = '{CosmosConstants.LessonTypeName}'");
            //var feedIterator = container.GetItemQueryIterator<CosmosLesson>(queryDefinition);

            //while (feedIterator.HasMoreResults)
            //{
            //    var response = await feedIterator.ReadNextAsync(cancellationToken);
            //    lessons.AddRange(response.Resource);
            //}

            //return lessons.Select(x => x.ToLessonDto()).ToList();
        }


        public async Task<LessonDto> CreateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
