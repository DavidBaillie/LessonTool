using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.Common.Domain.Utilities;
using LessonTool.UI.Infrastructure.Constants;
using Microsoft.AspNetCore.WebUtilities;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class LessonApiService : ApiServiceBase<LessonDto>, ILessonRepository
{
    public LessonApiService(IServiceProvider serviceProvider) 
        : base(serviceProvider, "LessonClient", ApiEndpointConstants.LessonsEndpoint)
    {

    }

    public async Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var query = QueryHelpers.AddQueryString(ApiEndpointConstants.LessonsEndpoint, new Dictionary<string, string> { { "min", $"{min}" }, { "max", $"{max}" } });
        var response = await client.GetAsync(query, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await HttpUtilities.DeserializeResponseAsync<List<LessonDto>>(response, jsonOptions, cancellationToken);
    }
}
