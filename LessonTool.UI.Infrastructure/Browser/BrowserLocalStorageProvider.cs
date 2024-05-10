using LessonTool.UI.Application.Interfaces;
using Microsoft.JSInterop;

namespace LessonTool.UI.Infrastructure.Browser;

public class BrowserLocalStorageProvider(IJSRuntime _jsRuntime) : IBrowserLocalStorage
{
    public async Task<string> GetValueOrDefaultAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var value = await _jsRuntime.InvokeAsync<string>("getFromLocalStorage", key);
            return value ?? string.Empty;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    public async Task<bool> TryDeleteValueAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("removeFromLocalStorage", key);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> TrySaveValueAsync(string key, string value, CancellationToken cancellationToken)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("addToLocalStorage", key, value);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
