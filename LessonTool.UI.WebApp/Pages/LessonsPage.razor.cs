using LessonTool.Common.Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class LessonsPage
{
    [Inject]
    private ILessonRepository lessonRepository { get; set; }
}
