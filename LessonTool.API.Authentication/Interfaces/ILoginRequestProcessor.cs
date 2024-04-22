using LessonTool.API.Authentication.Models;
using LessonTool.Common.Domain.Models.Authentication;

namespace LessonTool.API.Authentication.Interfaces;

public interface ILoginRequestProcessor
{
    AccessTokensResponseModel ProcessAnonymousLoginRequest();
    AccessTokensResponseModel ProcessAnonymousRefreshRequest(RefreshTokensRequestModel model);
    AccessTokensResponseModel ProcessUserAccountLoginRequest(UserAccount user, LoginRequestModel request);
    AccessTokensResponseModel ProcessUserAccountRefreshRequest(UserAccount user, RefreshTokensRequestModel model);
}