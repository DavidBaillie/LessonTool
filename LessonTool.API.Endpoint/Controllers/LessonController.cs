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
        private readonly ISectionRepository _sectionRepository;

        public LessonController(ILessonRepository lessonRepository, ISectionRepository sectionRepository)
        {
            _lessonRepository = lessonRepository;
            _sectionRepository = sectionRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<LessonDto>>> GetAllAsync(DateTime? min = null, DateTime? max = null, bool includeSections = false, CancellationToken cancellationToken = default)
        {
            var lessons = await _lessonRepository.GetLessonsAsync(min, max, cancellationToken);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetAsync(Guid id, bool includeSections = true, CancellationToken cancellationToken = default)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(id, cancellationToken);

            if (lesson is null || !includeSections)
                return Ok(lesson);

            lesson.Sections = await _sectionRepository.GetSectionsByLesson(lesson.Id, cancellationToken);
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<ActionResult<LessonDto>> PostAsync([FromBody] LessonDto lesson, CancellationToken cancellationToken)
        {
            var createdLesson = await _lessonRepository.CreateLessonAsync(lesson, cancellationToken);
            return CreatedAtAction(nameof(GetAsync), new { createdLesson.Id }, createdLesson);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromBody] LessonDto lesson, CancellationToken cancellationToken)
        {
            await _lessonRepository.UpdateLessonAsync(lesson, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _lessonRepository.DeleteLessonAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
