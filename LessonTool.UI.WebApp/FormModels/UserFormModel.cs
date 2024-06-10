using System.ComponentModel.DataAnnotations;

namespace LessonTool.UI.WebApp.FormModels;

public class UserFormModel
{
    public Guid Id { get; set; }

    [MinLength(1)]
    public string Username { get; set; }

    [MinLength(1)]
    public string UserRole { get; set; }
}
