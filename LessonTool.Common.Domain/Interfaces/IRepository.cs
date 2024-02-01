namespace LessonTool.Common.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets a single T from the repository by Id
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <param name="cancellationToken">Process token</param>
    Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new T in the repository
    /// </summary>
    /// <param name="entity">Entity to create</param>
    /// <param name="cancellationToken">Process token</param>
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates an existing T in the repository
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="cancellationToken">Process token</param>
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes an existing entity by Id
    /// </summary>
    /// <param name="id">Id of entity to delete</param>
    /// <param name="cancellationToken">Process token</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
