using LessonTool.Common.Domain.Models;

namespace LessonTool.Common.Domain.Extensions
{
    public static class SectionExtensions
    {
        public static SectionDto ToSectionDto(this CosmosSection section)
        {
            return new SectionDto()
            {
                Id = section.Id,
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
                Title = section.Title,
                Content = section.Content,
                CreatedDate = section.CreatedDate
            };
        }
    }
}
