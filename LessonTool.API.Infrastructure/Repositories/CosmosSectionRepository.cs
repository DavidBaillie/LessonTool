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


        public async Task<List<SectionDto>> GetSectionsByLesson(Guid sectionId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<SectionDto> CreateSectionAsync(SectionDto section, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<SectionDto> UpdateSectionAsync(SectionDto section, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task DeleteSectionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
