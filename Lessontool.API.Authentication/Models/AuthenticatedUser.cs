namespace Lessontool.API.Authentication.Models;

public class AuthenticatedUser
{
    public string UserName { get; set; }
    public string HashedPassword { get; set; }
    public string Salt { get; set; }
}
