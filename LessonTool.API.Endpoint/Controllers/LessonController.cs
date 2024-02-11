using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[Route("api/lessons")]
[ApiController]
public class LessonController : ApiControllerBase<LessonDto>
{
    private ILessonRepository _lessonRepository;

    public LessonController(ILessonRepository lessonRepository) 
        : base(lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LessonDto>>> GetAllAsync(DateTime? min = null, DateTime? max = null, bool includeSections = false, CancellationToken cancellationToken = default)
    {
        var lessons = await _lessonRepository.GetAllInDateRangeAsync(min, max, cancellationToken);
        return Ok(lessons);
    }
}
