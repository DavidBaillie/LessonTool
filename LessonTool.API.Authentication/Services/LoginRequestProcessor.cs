using LessonTool.API.Authentication.Exceptions;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using LessonTool.API.Domain.Interfaces;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using System.Security.Authentication;

namespace LessonTool.API.Authentication.Services;

public class LoginRequestProcessor(ILoginSessionRepository _loginSessions, ITokenGenerationService _tokenGenerator, IHashService _hashService) 
    : ILoginRequestProcessor
{
    private const int TokenExpiresMinutes = 120;
    private const int TokenExpiresMinutesThreshold = -5;

    /// <summary>
    /// Generates a token set for the anonymous user to read only with limit access
    /// </summary>
    public async Task<AccessTokensResponseModel> ProcessAnonymousLoginRequest(CancellationToken cancellationToken)
    {
        //Create tokens
        var tokens = CreateAnonymousAccessTokens(TokenExpiresMinutes);

        //Save the session to the db
        await _loginSessions.CreateAsync(
            new UserLoginSession()
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresDateTime = tokens.Expires,
                UserAccountId = Guid.Empty,
            }, cancellationToken);

        return tokens;
    }

    /// <summary>
    /// Handles taking an expired access token and a refresh token to create a new pair if the state data is correct
    /// </summary>
    /// <param name="model">Refresh tokens model</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns></returns>
    /// <exception cref="AuthenticationFailureException"></exception>
    /// <exception cref="NotExpiredException"></exception>
    public async Task<AccessTokensResponseModel> ProcessAnonymousRefreshRequest(RefreshTokensRequestModel model, CancellationToken cancellationToken)
    {
        //Grab session for anonymous user
        var loginSession = (await _loginSessions.GetSessionByUserIdAsync(Guid.Empty.ToString(), model.RefreshToken, cancellationToken))
            ?? throw new AuthenticationException("No login sessions for that user");

        //Check if token expired
        if (loginSession.ExpiresDateTime.AddMinutes(TokenExpiresMinutesThreshold) > DateTime.UtcNow)
            throw new NotExpiredException("Token not expired!");

        //Create new tokens for user
        var tokens = CreateAnonymousAccessTokens(TokenExpiresMinutesThreshold);

        //Save new tokens for this session
        loginSession.RefreshToken = tokens.RefreshToken;
        loginSession.AccessToken = tokens.AccessToken;
        loginSession.ExpiresDateTime = tokens.Expires;
        await _loginSessions.UpdateAsync(loginSession, cancellationToken);

        return tokens;
    }

    /// <summary>
    /// Generates a token set for request user if validation passes
    /// </summary>
    /// <param name="request">Login request</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns></returns>
    /// <exception cref="AuthenticationFailureException"></exception>
    public async Task<AccessTokensResponseModel> ProcessUserAccountLoginRequest(UserAccount user, LoginRequestModel request, CancellationToken cancellationToken)
    {
        var hashedPassword = _hashService.HashStringWithSalt(request.HashedPassword, Convert.FromBase64String(user.PasswordSalt));
        if (hashedPassword != user.Password)
            throw new AuthenticationException("Password mismatch, login failed");

        //Create tokens
        var tokens = CreateUserAccessTokens(user, TokenExpiresMinutes);

        //Save the session to the db
        var session = new UserLoginSession()
        {
            AccessToken = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken,
            ExpiresDateTime = tokens.Expires,
            UserAccountId = user.Id,
        };
        await _loginSessions.CreateAsync(session, cancellationToken);

        return tokens;
    }

    /// <summary>
    /// Handles taking an expired access token and a refresh token to create a new pair if the state data is correct
    /// </summary>
    /// <param name="username">Username of user to refresh</param>
    /// <param name="model">Token model</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns></returns>
    /// <exception cref="AuthenticationFailureException"></exception>
    /// <exception cref="NotExpiredException"></exception>
    public async Task<AccessTokensResponseModel> ProcessUserAccountRefreshRequest(UserAccount user, RefreshTokensRequestModel model, CancellationToken cancellationToken)
    {
        //Grab session for user
        var loginSession = (await _loginSessions.GetSessionByUserIdAsync(user.Id.ToString(), model.RefreshToken, cancellationToken))
            ?? throw new AuthenticationException("No login sessions for that user");

        //Check if token expired
        if (loginSession.ExpiresDateTime.AddMinutes(TokenExpiresMinutesThreshold) > DateTime.UtcNow)
            throw new NotExpiredException("Token not expired!");

        //Create new tokens for user
        var tokens = CreateUserAccessTokens(user, TokenExpiresMinutesThreshold);

        //Save new tokens for this session
        loginSession.RefreshToken = tokens.RefreshToken;
        loginSession.AccessToken = tokens.AccessToken;
        loginSession.ExpiresDateTime = tokens.Expires;
        await _loginSessions.UpdateAsync(loginSession, cancellationToken);

        return tokens;
    }

    /// <summary>
    /// Create an access token for the provided user
    /// </summary>
    /// <param name="user">User to create token for</param>
    /// <param name="expiresMinutes">Minutes until the token expires</param>
    /// <returns></returns>
    private AccessTokensResponseModel CreateUserAccessTokens(UserAccount user, int expiresMinutes)
    {
        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);
        var refreshToken = _tokenGenerator.CreateRefreshToken();
        var accessToken = _tokenGenerator.WriteSecurityToken(
            _tokenGenerator.CreateJwtSecurityToken(
                _tokenGenerator.CreateSigningCredentials(),
                _tokenGenerator.CreateUserClaims(user), expiresMinutes));

        return new AccessTokensResponseModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = expires
        };
    }

    /// <summary>
    /// Create an access token for an anonymous user 
    /// </summary>
    /// <param name="expiresMinutes">Minutes until the token expires</param>
    /// <returns></returns>
    private AccessTokensResponseModel CreateAnonymousAccessTokens(int expiresMinutes)
    {
        var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);
        var refreshToken = _tokenGenerator.CreateRefreshToken();
        var accessToken = _tokenGenerator.WriteSecurityToken(
            _tokenGenerator.CreateJwtSecurityToken(
                _tokenGenerator.CreateSigningCredentials(),
                _tokenGenerator.CreateAnonymousClaims(), expiresMinutes));

        return new AccessTokensResponseModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = expires
        };
    }
}