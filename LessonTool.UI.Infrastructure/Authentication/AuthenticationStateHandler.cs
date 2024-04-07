using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Infrastructure.Interfaces;

namespace LessonTool.UI.Infrastructure.Authentication;

public class AuthenticationStateHandler(IAuthenticationEndpointProvider _authenticationEndpoint, IPersistentStorage _storage)
{
    private const string accessTokenKey = "";
    private const string remeberSessionKey = "";

    private AccessTokensResponseModel tokens;

    public string AccessToken => tokens is not null ? tokens.AccessToken : throw new ApplicationException("No Access token available!");


    public async Task InitializeAuthenticationState(CancellationToken cancellationToken = default)
    {
        //Grab the remeber state and clear token if needed
        var rememberSessionValue = await _storage.GetValueOrDefaultAsync(remeberSessionKey, cancellationToken);
        if (string.IsNullOrEmpty(rememberSessionValue) || rememberSessionValue.Equals("false", StringComparison.InvariantCultureIgnoreCase))
        {
            await _storage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
        }

        var accessToken = await _storage.GetValueOrDefaultAsync(accessTokenKey, cancellationToken);
        if (string.IsNullOrEmpty(accessToken))
        {
            tokens = await GetAnonymousSessionToken(cancellationToken);
            await _storage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
            await _storage.TrySaveValueAsync(remeberSessionKey, "true", cancellationToken);
        }
        else
        {
            tokens = accessToken.StringtoTokens();

            //Token expired, grab a new anonymous session
            if (tokens.Expires < DateTime.UtcNow)
            {
                await _storage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
                tokens = await GetAnonymousSessionToken(cancellationToken);
                await _storage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
                await _storage.TrySaveValueAsync(remeberSessionKey, "true", cancellationToken);
            }
        }
    }

    private async Task<AccessTokensResponseModel> GetAnonymousSessionToken(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task<AccessTokensResponseModel> GetRefreshedUserSession(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
