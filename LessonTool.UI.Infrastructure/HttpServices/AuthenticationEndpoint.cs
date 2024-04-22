using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using LessonTool.UI.Application.Exceptions;
using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.Interfaces;
using System.Text.Json;
using static LessonTool.Common.Domain.Utilities.HttpUtilities;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class AuthenticationEndpoint(IHttpClientFactory _clientFactory, IHashService _hashService)  
    : IAuthenticationEndpoint
{
    protected JsonSerializerOptions jsonOptions { get; set; } = new(JsonSerializerDefaults.Web);

    public async Task<AccessTokensResponseModel> LoginAsAnonymousUserAsync(CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);
        var response = await client.PostAsync($"{ApiEndpointConstants.AuthenticationEndpoint}/login", 
            GeneratePayload(new LoginRequestModel()
            {
                Username = TokenConstants.AnonymousAccountToken.ToString(),
                HashedPassword = TokenConstants.AnonymousAccountToken.ToString(),
                RequestToken = TokenConstants.AuthenticationRequestToken,
            }, jsonOptions));

        if (!response.IsSuccessStatusCode)
            throw new BadHttpResponseException(response);

        var tokens = await DeserializeResponseAsync<AccessTokensResponseModel>(response, jsonOptions, cancellationToken);
        return tokens;
    }

    public async Task<AccessTokensResponseModel> LoginAsUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);
        var response = await client.PostAsync($"{ApiEndpointConstants.AuthenticationEndpoint}/login", 
            GeneratePayload(new LoginRequestModel()
            {
                Username = username,
                HashedPassword = _hashService.HashString(password),
                RequestToken = TokenConstants.AuthenticationRequestToken
            }, jsonOptions));

        if (!response.IsSuccessStatusCode)
            throw new BadHttpResponseException(response);

        var tokens = await DeserializeResponseAsync<AccessTokensResponseModel>(response, jsonOptions, cancellationToken);
        return tokens;
    }

    public async Task<AccessTokensResponseModel> RefreshSessionAsync(RefreshTokensRequestModel currentTokens, CancellationToken cancellationToken)
    {
        using var client = _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);
        var response = await client.PutAsync($"{ApiEndpointConstants.AuthenticationEndpoint}/refresh",
            GeneratePayload(new RefreshTokensRequestModel()
            {
                Token = currentTokens.Token,
                RefreshToken = currentTokens.RefreshToken,
                RequestToken = TokenConstants.AuthenticationRequestToken
            }, jsonOptions));

        if (!response.IsSuccessStatusCode)
            throw new BadHttpResponseException(response);

        var tokens = await DeserializeResponseAsync<AccessTokensResponseModel>(response, jsonOptions, cancellationToken);
        return tokens;
    }
}
