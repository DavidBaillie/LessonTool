using LessonTool.API.Infrastructure.Constants;
using Newtonsoft.Json;

namespace LessonTool.API.Infrastructure.Models;

public class CosmosLesson
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; } = CosmosConstants.LessonTypeName;
    public DateTime CreatedDate { get; set; }
    public DateTime VisibleDate { get; set; }
}
