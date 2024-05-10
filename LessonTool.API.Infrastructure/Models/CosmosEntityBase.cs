using Newtonsoft.Json;

namespace LessonTool.API.Infrastructure.Models;

public abstract class CosmosEntityBase
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "Type")]
    public string Type { get; set; }
}
