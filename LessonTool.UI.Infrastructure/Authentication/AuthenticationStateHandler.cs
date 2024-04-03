using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Infrastructure.Interfaces;

namespace LessonTool.UI.Infrastructure.Authentication;

public class AuthenticationStateHandler(IAuthenticationEndpointProvider _authenticationEndpoint, IPersistentStorage _storage)
{
    public async Task InitializeAuthenticationState()
    {

    }
}
