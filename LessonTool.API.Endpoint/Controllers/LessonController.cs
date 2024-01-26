using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private ILessonRepository _lessonRepository;

        public LessonController(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }


        [HttpGet]
        public async Task<ActionResult<LessonDto>> GetAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<LessonDto>> PostAsync([FromBody] LessonDto lesson, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LessonDto>> PutAsync(Guid id, [FromBody] LessonDto lesson)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            return NoContent();
        }
    }
}
