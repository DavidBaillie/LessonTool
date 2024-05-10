using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.Common.Domain.Exceptions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.Repositories;

public class EFCosmosLessonRepository(CosmosDbContext _context) : ILessonRepository
{
    public async Task<LessonDto> CreateAsync(LessonDto entity, CancellationToken cancellationToken = default)
    {
        var cosmosEntity = entity.ToCosmosLesson();
        cosmosEntity.Id = Guid.NewGuid().ToString();

        var dbEntry = await _context.Lessons.AddAsync(cosmosEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return dbEntry.Entity.ToLessonDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);
     
        if (lesson != null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
    {
        var minVisibleDate = min ?? DateTime.MinValue;
        var maxVisibleDate = max ?? DateTime.MaxValue;  

        var cosmosLessons = await _context.Lessons
            .Where(x => x.VisibleDate > minVisibleDate && x.VisibleDate < maxVisibleDate)
            .ToListAsync(cancellationToken);

        return cosmosLessons.Select(x => x.ToLessonDto()).ToList();
    }

    public async Task<LessonDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.Id == id.ToString()) 
            ?? throw new DataAccessException($"Failed to find a lesson with the matching Id [{id}]");

        return lesson.ToLessonDto();
    }

    public async Task<LessonDto> UpdateAsync(LessonDto entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            throw new DataAccessException($"Cannot update an entity when no Id was provided!");

        var lesson = entity.ToCosmosLesson();

        var entry = _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToLessonDto();
    }
}