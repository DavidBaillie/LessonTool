namespace LessonTool.Common.Domain.Interfaces;

public interface IRepository<T> : IReadOnlyInterface<T>, IWriteOnlyRepository<T> { }
