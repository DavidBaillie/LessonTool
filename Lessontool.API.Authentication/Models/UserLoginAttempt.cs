namespace Lessontool.API.Authentication.Models;

public class UserLoginAttempt
{
    public string Username { get; set; }
    public string HashedPassword { get; set; }
}
