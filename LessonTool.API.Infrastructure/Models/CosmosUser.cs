namespace LessonTool.API.Infrastructure.Models;

public class CosmosUser
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}
