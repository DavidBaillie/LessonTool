using LessonTool.Common.Domain.Models.Authentication;

namespace LessonTool.UI.Infrastructure.Interfaces
{
    public interface IAuthenticationEndpointProvider
    {
        Task<AccessTokensResponseModel> LoginAsAnonymousUserAsync(CancellationToken cancellationToken);
        Task<AccessTokensResponseModel> LoginAsUserAsync(string username, string password, CancellationToken cancellationToken);
        Task<AccessTokensResponseModel> RefreshSessionAsync(RefreshTokensRequestModel currentTokens, CancellationToken cancellationToken);
    }
}