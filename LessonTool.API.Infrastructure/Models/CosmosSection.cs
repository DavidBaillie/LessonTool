﻿using LessonTool.API.Infrastructure.Constants;
using Newtonsoft.Json;

namespace LessonTool.API.Infrastructure.Models;

public class CosmosSection
{
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; } = CosmosConstants.SectionTypeName;
    public DateTime CreatedDate { get; set; }
}
