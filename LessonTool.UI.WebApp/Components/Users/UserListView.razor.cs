using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Components.Users;

public partial class UserListView
{
    [Parameter, EditorRequired]
    public IEnumerable<UserDto> Users { get; set; }

    [Parameter]
    public EventCallback<Guid> OnSelectEditUser { get; set; }

    private async Task OnEditUser(Guid id)
    {
        if (OnSelectEditUser.HasDelegate)
            await OnSelectEditUser.InvokeAsync(id);
    }
}
