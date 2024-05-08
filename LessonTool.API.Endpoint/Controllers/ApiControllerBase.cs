using LessonTool.API.Authentication.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[Authorize]
public abstract class ApiControllerBase<T> : ControllerBase where T : EntityDtoBase
{
    private readonly IRepository<T> _repository;

    public ApiControllerBase(IRepository<T> repository)
    {
        _repository = repository;
    }

    [Authorize(Policy = PolicyNameConstants.ReaderPolicy)]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<T>> GetAsync(Guid id, bool includeSections = true, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetAsync(id, cancellationToken);
        return Ok(entity);
    }

    [Authorize(Policy = PolicyNameConstants.TeacherPolicy)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public virtual async Task<ActionResult<T>> PostAsync([FromBody] T inboundEntity, CancellationToken cancellationToken)
    {
        var entity = await _repository.CreateAsync(inboundEntity, cancellationToken);
        return CreatedAtAction(nameof(GetAsync), new { entity.Id }, entity);
    }

    [Authorize(Policy = PolicyNameConstants.TeacherPolicy)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult> PutAsync([FromBody] T inboundEntity, CancellationToken cancellationToken)
    {
        var entity = await _repository.UpdateAsync(inboundEntity, cancellationToken);
        return Ok(entity);
    }

    [Authorize(Policy = PolicyNameConstants.TeacherPolicy)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public virtual async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}