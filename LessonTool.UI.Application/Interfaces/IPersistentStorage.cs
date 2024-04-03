namespace LessonTool.UI.Application.Interfaces;

public interface IPersistentStorage
{
    Task<T> GetItemAsync<T>(string key);
    Task SaveItemAsync<T>(string key, T value);
}
