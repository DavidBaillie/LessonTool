using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.FormModels;

namespace LessonTool.UI.WebApp.Extensions;

public static class SectionModelExtensions
{
    public static SectionFormModel ToSectionFormModel(this SectionDto section)
    {
        return new SectionFormModel()
        {
            Id = section.Id,
            LessonId = section.LessonId,
            Content = section.Content,
            Title = section.Title,
            CreatedDate = section.CreatedDate
        };
    }

    public static SectionDto ToSectionDto(this SectionFormModel section, Guid? lessonId = null)
    {
        return new SectionDto()
        {
            Id = section.Id,
            LessonId = lessonId.HasValue ? lessonId.Value : section.LessonId,
            Content = section.Content,
            Title = section.Title,
            CreatedDate = section.CreatedDate
        };
    }
}
