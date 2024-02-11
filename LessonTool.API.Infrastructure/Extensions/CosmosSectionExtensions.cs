using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Extensions;

public static class CosmosSectionExtensions
{
    public static SectionDto ToSectionDto(this CosmosSection section)
    {
        return new SectionDto()
        {
            Id = section.Id,
            LessonId = section.LessonId,
            Title = section.Title,
            Content = section.Content,
            CreatedDate = section.CreatedDate
        };
    }

    public static CosmosSection ToCosmosSection(this SectionDto section)
    {
        return new CosmosSection()
        {
            Id = section.Id,
            LessonId = section.LessonId,
            Title = section.Title,
            Content = section.Content,
            CreatedDate = section.CreatedDate
        };
    }
}
