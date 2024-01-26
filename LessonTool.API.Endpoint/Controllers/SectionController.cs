using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Http;
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


        [HttpGet]
        public async Task<ActionResult<SectionDto>> GetAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SectionDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<SectionDto>> PostAsync([FromBody] SectionDto section, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SectionDto>> PutAsync(Guid id, [FromBody] SectionDto section)
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
