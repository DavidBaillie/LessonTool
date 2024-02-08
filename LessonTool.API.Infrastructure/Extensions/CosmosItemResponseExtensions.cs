using Microsoft.Azure.Cosmos;

namespace LessonTool.Common.Domain.Extensions;

public static class CosmosItemResponseExtensions
{
    public static bool IsSuccess<T>(this ItemResponse<T> response) => response != null && response.StatusCode == System.Net.HttpStatusCode.OK;
    public static bool IsCreated<T>(this ItemResponse<T> response) => response != null && response.StatusCode == System.Net.HttpStatusCode.Created;
}
