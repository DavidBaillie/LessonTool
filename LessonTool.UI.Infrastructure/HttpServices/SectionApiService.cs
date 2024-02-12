using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.Common.Domain.Utilities;
using LessonTool.UI.Infrastructure.Constants;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class SectionApiService : ApiServiceBase<SectionDto>, ISectionRepository
{
    public SectionApiService(IServiceProvider serviceProvider) 
        : base(serviceProvider, ApiEndpointConstants.CommonApiClientName, ApiEndpointConstants.SectionsEndpoint)
    {

    }

    public async Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.GetAsync($"{ApiEndpointConstants.LessonsEndpoint}/{lessonId}/sections", cancellationToken);
        response.EnsureSuccessStatusCode();

        return await HttpUtilities.DeserializeResponseAsync<List<SectionDto>>(response, jsonOptions, cancellationToken);
    }
}
