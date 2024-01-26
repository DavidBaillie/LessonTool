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
        public async Task<ActionResult<LessonDto>> GetAsync(DateTime? min = null, DateTime? max = null, bool includeSections = false, CancellationToken cancellationToken = default)
        {
            var lessons = await _lessonRepository.GetLessonsAsync(min, max, cancellationToken);
            
            //if (includeSections)
                //lessons.ForEach(x => )

            return Ok(lessons);
        }


        private async Task AssignSectionsToLesson(LessonDto lesson)
        {

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var lesson = await _lessonRepository.GetLessonByIdAsync(id, cancellationToken);
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<ActionResult<LessonDto>> PostAsync([FromBody] LessonDto lesson, CancellationToken cancellationToken)
        {
            var createdLesson = await _lessonRepository.CreateLessonAsync(lesson, cancellationToken);
            return CreatedAtAction("api/Lessons", new { createdLesson.Id }, createdLesson);
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
