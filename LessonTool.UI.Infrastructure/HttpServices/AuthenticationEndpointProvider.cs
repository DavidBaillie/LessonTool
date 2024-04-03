using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Infrastructure.Constants;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class AuthenticationEndpointProvider(IHttpClientFactory _clientFactory) 
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
