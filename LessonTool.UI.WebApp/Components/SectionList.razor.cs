﻿using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components;

public partial class SectionList
{
    [Parameter]
    public Guid LessonId { get; set; }

    [Parameter]
    public ISectionRepository SectionsRepository { get; set; }

    private List<SectionDto> sections = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();
            sections = await SectionsRepository.GetSectionsByLessonAsync(LessonId, cancellationToken);
        }
        catch (Exception ex)
        {

        }
    }
}
