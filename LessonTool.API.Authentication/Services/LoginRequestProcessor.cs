using LessonTool.API.Authentication.Exceptions;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;

namespace LessonTool.API.Authentication.Services;

public class LoginRequestProcessor(ITokenGenerationService _tokenGenerator, IHashService _hashService) 
    : ILoginRequestProcessor
{
    private const int TokenExpiresMinutes = 120;
    private const int ExpiredTokenThreshold = -5;

    /// <summary>
    /// Generates a token set for the anonymous user to read only with limit access
    /// </summary>
    public AccessTokensResponseModel ProcessAnonymousLoginRequest()
        => CreateAnonymousAccessTokens();

    /// <summary>
    /// Handles taking an expired access token and a refresh token to create a new pair if the state data is correct
    /// </summary>
    /// <param name="model">Refresh tokens model</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns></returns>
    /// <exception cref="AuthenticationFailureException"></exception>
    /// <exception cref="NotExpiredException"></exception>
    public AccessTokensResponseModel ProcessAnonymousRefreshRequest(RefreshTokensRequestModel model)
    {
        var token = new JwtSecurityToken(model.Token);

        //Check if token expired
        if (token.ValidTo.AddMinutes(ExpiredTokenThreshold) > DateTime.UtcNow)
            throw new NotExpiredException("Token not expired!");

        //Create new tokens for user
        return CreateAnonymousAccessTokens();
    }

    /// <summary>
    /// Generates a token set for request user if validation passes
    /// </summary>
    /// <param name="request">Login request</param>
    /// <param name="cancellationToken">Process token</param>
    /// <returns></returns>
    /// <exception cref="AuthenticationFailureException"></exception>
    public AccessTokensResponseModel ProcessUserAccountLoginRequest(UserAccount user, LoginRequestModel request)
    {
        var hashedPassword = _hashService.HashStringWithSalt(request.HashedPassword, Convert.FromBase64String(user.PasswordSalt));
        if (hashedPassword != user.Password)
            throw new AuthenticationException("Password mismatch, login failed");

        //Create tokens
        return CreateUserAccessTokens(user);
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
    public AccessTokensResponseModel ProcessUserAccountRefreshRequest(UserAccount user, RefreshTokensRequestModel model)
    {
        var token = new JwtSecurityToken(model.Token);

        //Check if token expired
        if (token.ValidTo.AddMinutes(ExpiredTokenThreshold) > DateTime.UtcNow)
            throw new NotExpiredException("Token not expired!");

        //Create new tokens for user
        return CreateUserAccessTokens(user);
    }

    /// <summary>
    /// Create an access token for the provided user
    /// </summary>
    /// <param name="user">User to create token for</param>
    /// <param name="expiresMinutes">Minutes until the token expires</param>
    /// <returns></returns>
    private AccessTokensResponseModel CreateUserAccessTokens(UserAccount user)
    {
        var expires = DateTime.UtcNow.AddMinutes(TokenExpiresMinutes);
        var refreshToken = _tokenGenerator.CreateRefreshToken();
        var accessToken = _tokenGenerator.WriteSecurityToken(
            _tokenGenerator.CreateJwtSecurityToken(
                _tokenGenerator.CreateSigningCredentials(),
                _tokenGenerator.CreateUserClaims(user), TokenExpiresMinutes));

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
    private AccessTokensResponseModel CreateAnonymousAccessTokens()
    {
        var expires = DateTime.UtcNow.AddMinutes(TokenExpiresMinutes);
        var refreshToken = _tokenGenerator.CreateRefreshToken();
        var accessToken = _tokenGenerator.WriteSecurityToken(
            _tokenGenerator.CreateJwtSecurityToken(
                _tokenGenerator.CreateSigningCredentials(),
                _tokenGenerator.CreateAnonymousClaims(), TokenExpiresMinutes));

        return new AccessTokensResponseModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = expires
        };
    }
}