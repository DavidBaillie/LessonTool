namespace LessonTool.Common.Domain.Interfaces;

public interface IReadOnlyInterface<T>
{
    /// <summary>
    /// Gets a single T from the repository by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <param name="cancellationToken">Process token</param>
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
