﻿using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class SectionItem
{
    [Parameter]
    public SectionDto Section{ get; set; }
}
