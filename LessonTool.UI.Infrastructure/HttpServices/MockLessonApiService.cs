using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class MockLessonApiService : ILessonRepository
{
    private List<LessonDto> lessons = new List<LessonDto>()
    {
        new LessonDto()
        {
            Id = new Guid("41ba1eef-2773-482a-8129-e51ce5531c69"),
            Name = "Lorem ipsum dolor sit",
            Description = "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa",
            VisibleDate = DateTime.MinValue,
            CreatedDate = DateTime.MinValue,
            Sections = new List<SectionDto>()
        },
        new LessonDto()
        {
            Id = new Guid("7023ece4-547d-4c44-95f1-1cc26782a085"),
            Name = "Quis nostrud exercitation ullamco",
            Description = "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa",
            VisibleDate = DateTime.MinValue,
            CreatedDate = DateTime.MinValue,
            Sections = new List<SectionDto>()
        }
    };

    public Task<LessonDto> CreateAsync(LessonDto entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();

        lessons.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lessons.RemoveAll(x => x.Id == id);
        return Task.CompletedTask;
    }

    public Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
    {
        var collection = lessons.Where(x => (min is null || x.CreatedDate > min) && (max is null || x.CreatedDate < max)).ToList();
        return Task.FromResult(collection);
    }

    public Task<LessonDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(lessons.FirstOrDefault(x => x.Id == id) ?? throw new HttpRequestException("Mock Failure, no Id found"));
    }

    public Task<LessonDto> UpdateAsync(LessonDto entity, CancellationToken cancellationToken = default)
    {
        if (!lessons.Any(x => x.Id == entity.Id))
            throw new HttpRequestException("Mock failure, no update id found");

        lessons.RemoveAll(x => x.Id == entity.Id);
        lessons.Add(entity);

        return Task.FromResult(entity);
    }
}