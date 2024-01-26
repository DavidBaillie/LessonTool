using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private ISectionRepository _sectionRepository;


        public SectionController(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var section = await _sectionRepository.GetSectionByIdAsync(id, cancellationToken);
            return Ok(section);
        }

        [HttpPost]
        public async Task<ActionResult<SectionDto>> PostAsync([FromBody] SectionDto section, CancellationToken cancellationToken)
        {
            var createdSection = await _sectionRepository.CreateSectionAsync(section, cancellationToken);
            return CreatedAtAction("api/sections", new { Id =  createdSection.Id }, createdSection);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync([FromBody] SectionDto section, CancellationToken cancellationToken)
        {
            await _sectionRepository.UpdateSectionAsync(section, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _sectionRepository.DeleteSectionAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
