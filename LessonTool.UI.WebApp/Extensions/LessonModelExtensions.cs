using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.FormModels;

namespace LessonTool.UI.WebApp.Extensions
{
    public static class LessonModelExtensions
    {
        public static LessonFormModel ToLessonFormModel(this LessonDto lesson, List<SectionDto> sections = null)
        {
            return new LessonFormModel()
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                PlannedDate = lesson.PlannedDate,
                VisibleDate = lesson.VisibleDate,
                Sections = sections is not null ? sections.Select(x => x.ToSectionFormModel()).ToList() : new()
            };
        }

        public static LessonDto ToLessonDto(this LessonFormModel lesson, List<SectionFormModel> sections = null)
        {
            return new LessonDto()
            {
                Id = lesson.Id,
                Name = lesson.Name,
                Description = lesson.Description,
                PlannedDate = lesson.PlannedDate,
                VisibleDate = lesson.VisibleDate,
                Sections = sections is not null ? sections.Select(x => x.ToSectionDto()).ToList() : new()
            };
        }
    }
}
