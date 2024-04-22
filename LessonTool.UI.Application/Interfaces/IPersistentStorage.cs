namespace LessonTool.UI.Application.Interfaces;

public interface IPersistentStorage
{
    Task<string> GetValueOrDefaultAsync(string key, CancellationToken cancellationToken);
    Task<bool> TrySaveValueAsync(string key, string value, CancellationToken cancellationToken);
    Task<bool> TryDeleteValueAsync(string key, CancellationToken cancellationToken);
}
