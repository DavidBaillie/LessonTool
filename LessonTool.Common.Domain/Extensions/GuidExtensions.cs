namespace LessonTool.Common.Domain.Extensions;

public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid) => !guid.HasValue || guid.Value == Guid.Empty;
    public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;
    public static bool IsNotEmpty(this Guid guid) => guid != Guid.Empty;
}
