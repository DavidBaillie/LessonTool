using LessonTool.UI.Infrastructure.Interfaces;
using System.Net.Http.Headers;

namespace LessonTool.UI.WebApp.Middleware;

public class AuthenticationTokenClientMiddleware(IAuthenticationStateHandler _authenticationStateHandler)
    : DelegatingHandler
{


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authenticationStateHandler.GetAccessTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
