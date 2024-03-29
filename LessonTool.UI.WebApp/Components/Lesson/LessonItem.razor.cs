﻿using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Lesson;

public partial class LessonItem
{
    [Parameter]
    public LessonDto Lesson { get; set; }

    private string lessonLink => $"/lesson?id={Lesson.Id}";

    private string lessonName { get => Lesson is null ? "N/A" : Lesson.Name; }
    private string lessonDescription { get => Lesson is null ? "N/A" : Lesson.Description; }
    private string lessonPlannedDatetime { get => Lesson is null ? "N/A" : Lesson.PlannedDate.ToString("dd/MM/yyyy"); }
}
