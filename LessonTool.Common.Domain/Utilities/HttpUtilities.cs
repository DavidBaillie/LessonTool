using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;

namespace LessonTool.Common.Domain.Utilities;

public static class HttpUtilities
{
    public static StringContent GeneratePayload<T>(T entity, JsonSerializerOptions jsonOptions)
    {
        return new StringContent(
            JsonSerializer.Serialize(entity, jsonOptions),
                Encoding.UTF8, "application/json");
    }

    public static async Task<T> DeserializeResponse<T>(HttpResponseMessage message, JsonSerializerOptions jsonOptions, CancellationToken cancellationToken = default)
    {
        var payload = await message.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(payload, jsonOptions);
    }

    public static Uri BuildQueryString(string baseEndpoint, Dictionary<string, string> queryParams)
    {
        if (queryParams == null || queryParams.Count == 0)
            return new Uri(baseEndpoint);

        return new Uri(QueryHelpers.AddQueryString(baseEndpoint, queryParams));
    }
}
