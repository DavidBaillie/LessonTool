
using LessonTool.Common.Domain.Interfaces;
using LessonTool.UI.WebApp.Extensions;
using LessonTool.UI.WebApp.FormModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LessonTool.UI.WebApp.Components.Users;

public partial class UserEditor
{
    [Parameter, EditorRequired]
    public Guid UserId { get; set; }

    [Parameter, EditorRequired]
    public IUserRepository UserRepository { get; set; }

    private UserFormModel userFormModel;
    private EditContext editContext;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (UserId == Guid.Empty)
        {
            userFormModel = new();
            editContext = new(userFormModel);
        }
        else
        {
            var user = await UserRepository.GetAsync(UserId);
            userFormModel = user.ToFormModel();
            editContext = new(userFormModel);
        }
    }

    private async Task<bool> TrySaveAsync()
    {
        if (!editContext.Validate())
            return false;

        try
        {
            if (userFormModel.Id == Guid.Empty)
            {
                await UserRepository.CreateAsync(userFormModel.ToDto());
            }
            else
            {
                await UserRepository.UpdateAsync(userFormModel.ToDto());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save the user: {ex}");
            return false;
        }

        return true;
    }
}
