using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using LessonTool.UI.Infrastructure.Constants;
using System.Text.Json;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class UsersEndpoint : ApiServiceBase<UserDto>, IUserRepository
{
    public UsersEndpoint(IServiceProvider serviceProvider)
        : base(serviceProvider, ApiEndpointConstants.CommonApiClientName, ApiEndpointConstants.UsersEndpoint)
    {

    }
}
