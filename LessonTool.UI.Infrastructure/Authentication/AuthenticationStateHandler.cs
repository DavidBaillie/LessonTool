using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Application.Exceptions;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Infrastructure.Interfaces;

namespace LessonTool.UI.Infrastructure.Authentication;

public class AuthenticationStateHandler(IAuthenticationEndpoint _authenticationEndpoint, IHashService _hashing, IBrowserLocalStorage _localStorage) 
    : IAuthenticationStateHandler
{
    private const string accessTokenKey = "at";
    private const string rememberSessionKey = "rs";

    private AccessTokensResponseModel tokens;

    //public string AccessToken => tokens?.AccessToken ?? throw new ApplicationException("No Access token available!");
    public string AccessToken
    {
        get
        {
            if (tokens is null) throw new ApplicationException($"No tokens are available to the web page!");
            if (string.IsNullOrEmpty(tokens.AccessToken)) throw new ApplicationException($"Tokens found, missing access token!");

            return tokens.AccessToken;
        }
    }

    /// <summary>
    /// Called during page startup, handles checking token states and generating the needed data to call the API
    /// </summary>
    /// <param name="cancellationToken">Process tokens</param>
    /// <returns></returns>
    public async Task InitializeAuthenticationStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            //Grab the remeber state and clear token if needed
            var rememberSessionValue = await _localStorage.GetValueOrDefaultAsync(rememberSessionKey, cancellationToken);
            if (string.IsNullOrEmpty(rememberSessionValue) || rememberSessionValue.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Should not remeber session - deleting key");
                await _localStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
            }

            var accessToken = await _localStorage.GetValueOrDefaultAsync(accessTokenKey, cancellationToken);

            //No token saved
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine($"No local access token, using anon");
                tokens = await _authenticationEndpoint.LoginAsAnonymousUserAsync(cancellationToken);

                await _localStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
                await _localStorage.TrySaveValueAsync(rememberSessionKey, true.ToString(), cancellationToken);
            }
            //Existing token
            else
            {
                tokens = accessToken.ParseToAccessTokens();

                //Try to refresh current tokens
                try
                {
                    tokens = await _authenticationEndpoint.RefreshSessionAsync(tokens.ToRefreshTokensModel(), cancellationToken);
                    Console.WriteLine("Token refreshed successfully");
                }
                catch (BadHttpResponseException ex)
                {
                    //Tokens still good
                    if ((int)ex.StatusCode == 425)
                    {
                        Console.WriteLine($"Current tokens are good!");
                    }
                    //Tokens not good, grab a new anon one
                    else
                    {
                        Console.WriteLine("Failed to refresh token, requesting new anonymous session");
                        tokens = await _authenticationEndpoint.LoginAsAnonymousUserAsync(cancellationToken);
                    }
                }

                await _localStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
                await _localStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to initialize tokens, website cannot function -> {e}");
        }
    }

    /// <summary>
    /// Provided login credentials, will try to generate access tokens for use in HTTP calls
    /// </summary>
    /// <param name="username">User display name</param>
    /// <param name="password">User plain-text password</param>
    /// <param name="rememberSession">If the session should be saved for next time page opens</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns>If the login was successful</returns>
    public async Task<bool> TryLoginUsingCredentialsAsync(string username, string password, bool rememberSession, CancellationToken cancellationToken)
    {
        try
        {
            tokens = await _authenticationEndpoint.LoginAsUserAsync(username, _hashing.HashString(password), cancellationToken);
            await _localStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
            await _localStorage.TrySaveValueAsync(rememberSessionKey, rememberSession.ToString(), cancellationToken);
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> TryLogout(CancellationToken cancellationToken)
    {
        try
        {
            tokens = await _authenticationEndpoint.LoginAsAnonymousUserAsync(cancellationToken);

            await _localStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
            await _localStorage.TryDeleteValueAsync(rememberSessionKey, cancellationToken);

            await _localStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
            await _localStorage.TrySaveValueAsync(rememberSessionKey, true.ToString(), cancellationToken);
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }
}
