﻿using LessonTool.UI.Application.Interfaces;
using Microsoft.JSInterop;

namespace LessonTool.UI.Infrastructure.Browser;

public class BrowserSessionStorageProvider(IJSRuntime _jsRuntime) : IBrowserSessionStorage
{
    public async Task<string> GetValueOrDefaultAsync(string key, CancellationToken cancellationToken)
    {
        try
        {
            var value = await _jsRuntime.InvokeAsync<string>("getFromSessionStorage", key);
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
            await _jsRuntime.InvokeVoidAsync("removeFromSessionStorage", key);
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
            await _jsRuntime.InvokeVoidAsync("addToSessionStorage", key, value);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
