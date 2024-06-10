using LessonTool.Common.Domain.Interfaces;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class UserEditorPage
{
    [Inject]
    public IUserRepository UserRepository { get; set; }

    private Guid userId;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!navigationManager.TryGetQueryString("Id", out Guid id, new(Guid.TryParse)))
            userId = id;
    }
}
