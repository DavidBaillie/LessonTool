using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class MockSectionRepositoryService : ISectionRepository
{
    private List<SectionDto> sections = new List<SectionDto>()
    {
        new SectionDto()
        {
            Id = Guid.NewGuid(),
            LessonId = new Guid("41ba1eef-2773-482a-8129-e51ce5531c69"),
            Title = "Excepteur sint occaecat",
            Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
            CreatedDate = DateTime.MinValue
        },
        new SectionDto()
        {
            Id = Guid.NewGuid(),
            LessonId = new Guid("41ba1eef-2773-482a-8129-e51ce5531c69"),
            Title = "Excepteur sint occaecat",
            Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
            CreatedDate = DateTime.MinValue
        },
        new SectionDto()
        {
            Id = Guid.NewGuid(),
            LessonId = new Guid("7023ece4-547d-4c44-95f1-1cc26782a085"),
            Title = "Excepteur sint occaecat",
            Content = "sunt in culpa qui officia deserunt mollit anim id est laborum",
            CreatedDate = DateTime.MinValue
        }
    };

    public Task<SectionDto> CreateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        sections.Add(entity);

        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        sections.RemoveAll(x =>  x.Id == id);
        return Task.CompletedTask;
    }

    public Task<SectionDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(sections.FirstOrDefault(x => x.Id == id) ?? throw new HttpRequestException("Mock failure"));
    }

    public Task<List<SectionDto>> GetSectionsByLessonAsync(Guid lessonId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(sections.Where(x => x.LessonId == lessonId).ToList());
    }

    public Task<SectionDto> UpdateAsync(SectionDto entity, CancellationToken cancellationToken = default)
    {
        if (!sections.Any(x => x.Id == entity.Id))
            throw new HttpRequestException("Mock failure");

        sections.RemoveAll(x => x.Id == entity.Id);
        sections.Add(entity);

        return Task.FromResult(entity);
    }
}