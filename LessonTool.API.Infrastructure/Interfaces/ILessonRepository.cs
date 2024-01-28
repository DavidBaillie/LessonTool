using LessonTool.Common.Domain.Models;

namespace LessonTool.API.Infrastructure.Interfaces
{
    public interface ILessonRepository
    {
        /// <summary>
        /// Creates a lesson object in persistent storage
        /// </summary>
        /// <param name="lesson">Lesson Dto to save</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns>Persistent version of Dto</returns>
        Task<LessonDto> CreateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Deletes any lesson with a matching Id
        /// </summary>
        /// <param name="id">Id to delete by</param>
        /// <param name="cancellationToken">Process token</param>
        Task DeleteLessonAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Returns a single lesson from persistent storage by Id
        /// </summary>
        /// <param name="id">Id of lesson</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns>Dto in storage</returns>
        Task<LessonDto> GetLessonByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Returns all lessons withing a given datetime range
        /// </summary>
        /// <param name="min">Minimum date</param>
        /// <param name="max">Maximum date</param>
        /// <param name="cancellationToken">Process token</param>
        Task<List<LessonDto>> GetLessonsAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates a given lesson in persistent storage 
        /// </summary>
        /// <param name="lesson">Lesson Dto to update</param>
        /// <param name="cancellationToken">Process token</param>
        /// <returns></returns>
        Task<LessonDto> UpdateLessonAsync(LessonDto lesson, CancellationToken cancellationToken = default);
    }
}