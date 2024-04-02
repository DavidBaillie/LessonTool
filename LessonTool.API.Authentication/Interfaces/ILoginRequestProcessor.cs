using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Models.Authentication;

namespace LessonTool.API.Authentication.Interfaces
{
    public interface ILoginRequestProcessor
    {
        Task<AccessTokensResponseModel> ProcessAnonymousLoginRequest(CancellationToken cancellationToken);
        Task<AccessTokensResponseModel> ProcessAnonymousRefreshRequest(RefreshTokensRequestModel model, CancellationToken cancellationToken);
        Task<AccessTokensResponseModel> ProcessUserAccountLoginRequest(UserAccount user, LoginRequestModel request, CancellationToken cancellationToken);
        Task<AccessTokensResponseModel> ProcessUserAccountRefreshRequest(UserAccount user, RefreshTokensRequestModel model, CancellationToken cancellationToken);
    }
}