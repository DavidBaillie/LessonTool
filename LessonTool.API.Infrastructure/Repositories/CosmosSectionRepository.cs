using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Repositories
{
    public class CosmosSectionRepository : ISectionRepository
    {
        private readonly ICosmosContainerFactory _containerFactory;


        public CosmosSectionRepository(ICosmosContainerFactory cosmosContainerFactory)
        {
            _containerFactory = cosmosContainerFactory;
        }


        public async Task<SectionDto> GetSectionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<List<LessonDto>> GetSectionsByLesson(Guid lessonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<LessonDto> CreateSectionAsync(LessonDto lesson, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<LessonDto> UpdateSectionAsync(LessonDto lesson, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task DeleteSectionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
