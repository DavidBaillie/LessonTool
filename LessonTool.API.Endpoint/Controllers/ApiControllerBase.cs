using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

public abstract class ApiControllerBase<T> : ControllerBase where T : EntityDtoBase
{
    private readonly IRepository<T> _repository;

    public ApiControllerBase(IRepository<T> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<T>> GetAsync(Guid id, bool includeSections = true, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(id, cancellationToken);
        return Ok(entity);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public virtual async Task<ActionResult<T>> PostAsync([FromBody] T inboundEntity, CancellationToken cancellationToken)
    {
        var entity = await _repository.CreateAsync(inboundEntity, cancellationToken);
        return CreatedAtAction(nameof(GetAsync), new { entity.Id }, entity);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult> PutAsync([FromBody] T inboundEntity, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(inboundEntity, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public virtual async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}