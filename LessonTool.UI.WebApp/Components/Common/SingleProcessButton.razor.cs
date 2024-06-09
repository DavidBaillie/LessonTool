using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Common;

public partial class SingleProcessButton
{
    [Parameter, EditorRequired]
    public EventCallback<Action> OnPressed { get; set; }

    [Parameter, EditorRequired]
    public string ButtonContent { get; set; } = "";

    [Parameter]
    public string CssClass { get; set; } = "";

    private bool buttonIsPressed = false;
    private string buttonClass => string.IsNullOrEmpty(CssClass) ? "btn btn-primary" : CssClass;

    private void PressButton()
    {
        if (buttonIsPressed || !OnPressed.HasDelegate) 
            return;

        buttonIsPressed = true;
        OnPressed.InvokeAsync(ReleaseButton);
    }

    private void ReleaseButton()
    {
        buttonIsPressed = false;
    }
}
