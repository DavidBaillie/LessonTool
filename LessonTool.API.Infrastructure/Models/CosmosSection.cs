﻿using LessonTool.API.Infrastructure.Constants;

namespace LessonTool.API.Infrastructure.Models;

public class CosmosSection
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; } = CosmosConstants.SectionTypeName;
    public DateTime CreatedDate { get; set; }
}
