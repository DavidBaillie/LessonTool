using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Models;
using System;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class LessonApiService : ApiServiceBase<LessonDto>, ILessonRepository
{
    private const string _lessonApiPath = "api/lessons";

    public LessonApiService(IServiceProvider serviceProvider) 
        : base(serviceProvider, "LessonClient", _lessonApiPath)
    {

    }

    public Task<List<LessonDto>> GetAllInDateRangeAsync(DateTime? min = null, DateTime? max = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
