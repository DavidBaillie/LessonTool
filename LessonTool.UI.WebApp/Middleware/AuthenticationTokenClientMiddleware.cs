using LessonTool.UI.Infrastructure.Interfaces;
using System.Net.Http.Headers;

namespace LessonTool.UI.WebApp.Middleware;

public class AuthenticationTokenClientMiddleware(IAuthenticationStateHandler _authenticationStateHandler)
    : DelegatingHandler
{


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _authenticationStateHandler.AccessToken;

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Console.WriteLine($"Added token to request: {token}");

        return await base.SendAsync(request, cancellationToken);
    }
}
