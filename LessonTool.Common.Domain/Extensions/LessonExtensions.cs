﻿using LessonTool.Common.Domain.Models;

namespace LessonTool.Common.Domain.Extensions;

public static class LessonExtensions
{
    public static LessonDto ToLessonDto(this CosmosLesson lesson, IEnumerable<CosmosSection> sections = null)
    {
        return new LessonDto()
        {
            Id = lesson.Id,
            Name = lesson.Name,
            Description = lesson.Description,
            CreatedDate = lesson.CreatedDate,
            VisibleDate = lesson.VisibleDate,
            Sections = sections is null ? new() : sections.Select(x => x.ToSectionDto()).ToList()
        };
    }

    public static CosmosLesson ToCosmosLesson(this LessonDto lesson)
    {
        return new CosmosLesson()
        {
            Id = lesson.Id,
            Name = lesson.Name,
            Description = lesson.Description,
            CreatedDate = lesson.CreatedDate,
            VisibleDate = lesson.VisibleDate
        };
    }
}
