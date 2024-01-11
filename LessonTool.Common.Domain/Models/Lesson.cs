using LessonTool.Common.Domain.Constants;

namespace LessonTool.Common.Domain.Models
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = CosmosConstants.LessonTypeName;
    }
}
