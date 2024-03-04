using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.Common.Domain.Exceptions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LessonTool.API.Infrastructure.Repositories;

public class EfCosmosSectionRepository(CosmosDbContext _context) : ISectionRepository
{
    public async Task<SectionDto> CreateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        var section = entity.ToCosmosSection();
        section.Id = Guid.NewGuid().ToString();

        var entry = await _context.Sections.AddAsync(section, cancellationToken);   
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToSectionDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var section = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);

        if (section is not null)
        {
            _context.Sections.Remove(section);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<SectionDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cosmosSection = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id.ToString(), cancellationToken);
        return cosmosSection.ToSectionDto();
    }

    public async Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        var lessonIdString = lessonId.ToString();
        var sections = await _context.Sections
            .Where(x => x.LessonId == lessonIdString)
            .ToListAsync(cancellationToken);
        return sections.Select(x => x.ToSectionDto()).ToList();
    }

    public async Task<SectionDto> UpdateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            throw new DataAccessException($"Cannot update a section when no Id provided!");

        var entry = _context.Sections.Update(entity.ToCosmosSection());
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToSectionDto();
    }
}