using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.Interfaces;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class AuthenticationEndpoint(IHttpClientFactory _clientFactory)  
    : IAuthenticationEndpoint
{
    public async Task<AccessTokensResponseModel> LoginAsAnonymousUserAsync(CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);

        return default;
    }

    public async Task<AccessTokensResponseModel> LoginAsUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);

        return default;
    }

    public async Task<AccessTokensResponseModel> RefreshSessionAsync(RefreshTokensRequestModel currentTokens, CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);

        return default;
    }
}
