using LessonTool.API.Authentication.Models;
using LessonTool.API.Infrastructure.Constants;
using LessonTool.API.Infrastructure.Models;

namespace LessonTool.API.Infrastructure.Extensions;

public static class CosmosLoginSessionExtensions
{
    public static CosmosLoginSession ToCosmosLoginSession(this UserLoginSession session)
    {
        return new CosmosLoginSession()
        {
            Id = session.Id.ToString(),
            Type = CosmosConstants.LoginSessionTypeName,
            UserAccountId = session.UserAccountId.ToString(),
            AccessToken = session.AccessToken.ToString(),
            RefreshToken = session.RefreshToken.ToString(),
            ExpiresDateTime = session.ExpiresDateTime
        };
    }

    public static UserLoginSession ToLoginSession(this CosmosLoginSession session)
    {
        return new UserLoginSession()
        {
            Id = new Guid(session.Id),
            UserAccountId = new Guid(session.UserAccountId),
            AccessToken = session.AccessToken,
            RefreshToken = session.RefreshToken,
            ExpiresDateTime = session.ExpiresDateTime
        };
    }
}
