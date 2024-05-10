using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Lesson;

public partial class LessonItem
{
    [Parameter]
    public LessonDto Lesson { get; set; }

    [Parameter]
    public bool IsAllowedToEdit {  get; set; }

    private string lessonLink => $"/lesson?id={Lesson.Id}";

    private string lessonName { get => Lesson is null ? "N/A" : Lesson.Name; }
    private string lessonDescription { get => Lesson is null ? "N/A" : Lesson.Description; }
    private string lessonPlannedDatetime { get => Lesson is null ? "N/A" : Lesson.VisibleDate.ToString("dd/MM/yyyy"); }
}
