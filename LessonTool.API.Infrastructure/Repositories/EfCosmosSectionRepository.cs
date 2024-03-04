using LessonTool.API.Infrastructure.EntityFramework;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using System.Data.Entity;

namespace LessonTool.API.Infrastructure.Repositories;

public class EfCosmosSectionRepository(CosmosDbContext _context) : ISectionRepository
{
    public async Task<SectionDto> CreateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        var cosmosEntry = await _context.Sections.AddAsync(entity.ToCosmosSection(), cancellationToken);   
        await _context.SaveChangesAsync(cancellationToken);

        return cosmosEntry.Entity.ToSectionDto();
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
        var sections = await _context.Sections.Where(x => x.LessonId == lessonId.ToString()).ToListAsync(cancellationToken);
        return sections.Select(x => x.ToSectionDto()).ToList();
    }

    public async Task<SectionDto> UpdateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        var entry = _context.Sections.Update(entity.ToCosmosSection());
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Entity.ToSectionDto();
    }
}