using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.WebApp.Extensions;
using Microsoft.AspNetCore.Components;

namespace LessonTool.UI.WebApp.Pages;

public partial class UserMaintenancePage
{
    [Inject]
    public IUserRepository UserRepository { get; set; }

    private string failureMessage;

    private List<UserDto> users = new List<UserDto>();
    private bool isLoadingFromSource = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            users = await UserRepository.GetAllAsync(cancellationToken);
            users = users.OrderBy(x => x.Username).ToList();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Http failure! {ex}");
            failureMessage = $"Failed to load lesson from Api: {ex.StatusCode}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize: {ex}");
            failureMessage = $"General failure: {ex}";
        }
        finally
        {
            isLoadingFromSource = false;
        }
    }
}
