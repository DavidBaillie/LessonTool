﻿using LessonTool.API.Authentication.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[Route("api/sections")]
[ApiController]
public class SectionController : ApiControllerBase<SectionDto>
{
    private ISectionRepository _sectionRepository;


    public SectionController(ISectionRepository sectionRepository)
        : base(sectionRepository) 
    {
        _sectionRepository = sectionRepository;
    }

    [Authorize(Policy = PolicyNameConstants.ReaderPolicy)]
    [HttpGet("/api/lessons/{id}/sections")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SectionDto>>> GetByLessonAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sections = await _sectionRepository.GetSectionsByLessonAsync(id, cancellationToken);
        return Ok(sections);
    }
}
