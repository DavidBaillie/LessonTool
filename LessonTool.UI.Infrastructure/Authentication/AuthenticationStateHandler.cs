using LessonTool.Common.Domain.Extensions;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Application.Exceptions;
using LessonTool.UI.Application.Interfaces;
using LessonTool.UI.Infrastructure.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace LessonTool.UI.Infrastructure.Authentication;

public class AuthenticationStateHandler(IAuthenticationEndpoint _authenticationEndpoint, IBrowserLocalStorage _localStorage, IBrowserSessionStorage _sessionStorage) 
    : IAuthenticationStateHandler
{
    private const string accessTokenKey = "at";
    private const string rememberSessionKey = "rs";

    private AccessTokensResponseModel tokens;
    private Task loadingTask = null;

    private IPersistentStorage persistentStorage = _localStorage;

    public event Func<Task> OnLoginStateChangedAsync;

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        //There is a loading task already, await it
        if (loadingTask != null)
        {
            await loadingTask;
            return tokens?.AccessToken ?? throw new ApplicationException($"No access token available to the project!");
        }

        Console.WriteLine($"First one here, grabbing tokens");

        //Create a task that can be awaited to generate a token from
        var taskSource = new TaskCompletionSource();
        loadingTask = taskSource.Task;

        try
        {
            //Setup the local tokens
            await InitializeAuthenticationStateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize state!");
        }

        //Done work, return
        taskSource.SetResult();
        return tokens.AccessToken;
    }

    public async Task<JwtSecurityToken> GetSecuritytokenAsync(CancellationToken cancellationToken)
    {
        var currentToken = await GetAccessTokenAsync(cancellationToken);
        return new JwtSecurityToken(currentToken);
    }

    /// <summary>
    /// Called during page startup, handles checking token states and generating the needed data to call the API
    /// </summary>
    /// <param name="cancellationToken">Process tokens</param>
    /// <returns></returns>
    private async Task InitializeAuthenticationStateAsync(CancellationToken cancellationToken)
    {
        try
        {
            //Grab the remeber state and clear token if needed
            var rememberSessionValue = await _localStorage.GetValueOrDefaultAsync(rememberSessionKey, cancellationToken);

            //No value or set to false
            if (string.IsNullOrEmpty(rememberSessionValue) || rememberSessionValue.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"Remember session FALSE");
                //Delete any key in the local storage
                await _localStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
                //Switch key storage to use session only
                persistentStorage = _sessionStorage;
            }

            var accessToken = await persistentStorage.GetValueOrDefaultAsync(accessTokenKey, cancellationToken);
            Console.WriteLine($"Loaded token {accessToken}");

            //No token saved
            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine($"No local token, getting new anonymous one");
                tokens = await _authenticationEndpoint.LoginAsAnonymousUserAsync(cancellationToken);

                await _localStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
                await _localStorage.TrySaveValueAsync(rememberSessionKey, true.ToString(), cancellationToken);

                await _sessionStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
            }
            //Existing token
            else
            {
                tokens = accessToken.ParseToAccessTokens();
                Console.WriteLine($"Found local tokens: {tokens.AccessToken}");

                //Try to refresh current tokens
                try
                {
                    tokens = await _authenticationEndpoint.RefreshSessionAsync(tokens.ToRefreshTokensModel(), cancellationToken);
                    Console.WriteLine($"Correctly refreshed tokens");
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

                Console.WriteLine($"Tokens good, updating local storage");
                await persistentStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
                await persistentStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
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
            tokens = await _authenticationEndpoint.LoginAsUserAsync(username, password, cancellationToken);

            //If remembering session, try to clear session and switch provider back to local storage
            persistentStorage = rememberSession ? _localStorage : _sessionStorage;

            await _sessionStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
            await _sessionStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);

            await persistentStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
            await _localStorage.TrySaveValueAsync(rememberSessionKey, rememberSession.ToString(), cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to login: {ex}");
            return false;
        }

        await (OnLoginStateChangedAsync?.Invoke() ?? Task.CompletedTask);
        return true;
    }

    public async Task<bool> TryLogoutAsync(CancellationToken cancellationToken)
    {
        try
        {
            tokens = await _authenticationEndpoint.LoginAsAnonymousUserAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return false;
        }

        persistentStorage = _localStorage;

        await _sessionStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);
        await _localStorage.TryDeleteValueAsync(accessTokenKey, cancellationToken);

        await persistentStorage.TrySaveValueAsync(accessTokenKey, tokens.TokensToString(), cancellationToken);
        await _localStorage.TrySaveValueAsync(rememberSessionKey, true.ToString(), cancellationToken);

        await (OnLoginStateChangedAsync?.Invoke() ?? Task.CompletedTask);
        return true;
    }
}
