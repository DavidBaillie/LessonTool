using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class SectionApiService : ApiServiceBase<SectionDto>, ISectionRepository
{
    private const string _sectionApiPath = "api/sections";

    public SectionApiService(IServiceProvider serviceProvider) 
        : base(serviceProvider, "SectionClient", _sectionApiPath)
    {

    }

    public Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
