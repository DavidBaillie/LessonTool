using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Common;

public partial class InlineErrorDisplay
{
    private string content = "";


    public void DisplayError(string message, float duration)
    {
        content = message;
        StateHasChanged();
        _ = HideAfterAsync((int)(duration * 1000));
    }

    private async Task HideAfterAsync(int duration)
    {
        await Task.Delay(duration);
        content = string.Empty;
        await InvokeAsync(StateHasChanged);
    }
}
