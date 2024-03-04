using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Extensions;

public static class CosmosLessonExtensions
{
    public static LessonDto ToLessonDto(this CosmosLesson lesson, IEnumerable<CosmosSection> sections = null)
    {
        return new LessonDto()
        {
            Id = new Guid(lesson.Id),
            Name = lesson.Name,
            Description = lesson.Description,
            PlannedDate = lesson.PlannedDate,
            VisibleDate = lesson.VisibleDate,
            Sections = sections is null ? new() : sections.Select(x => x.ToSectionDto()).ToList()
        };
    }

    public static CosmosLesson ToCosmosLesson(this LessonDto lesson)
    {
        return new CosmosLesson()
        {
            Id = lesson.Id.ToString(),
            Type = CosmosConstants.LessonTypeName,
            Name = lesson.Name,
            Description = lesson.Description,
            PlannedDate = lesson.PlannedDate,
            VisibleDate = lesson.VisibleDate
        };
    }
}
