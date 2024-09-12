using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LessonTool.API.Endpoint.Test.Authentication;

internal class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private IList<Claim> Claims { get; set; } = new List<Claim>();

    public AuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory factory, UrlEncoder encoder,
        ClaimsProvider claimsProvider) : base(options, factory, encoder)
    {
        Claims = claimsProvider.Claims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(Claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
