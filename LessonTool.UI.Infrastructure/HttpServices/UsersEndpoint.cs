using LessonTool.UI.Infrastructure.Constants;
using LessonTool.UI.Infrastructure.Interfaces;
using System.Text.Json;

namespace LessonTool.UI.Infrastructure.HttpServices;

public class UsersEndpoint(IAuthenticationStateHandler _authenticationStateHandler, IHttpClientFactory _clientFactory)
{
    private JsonSerializerOptions jsonOptions { get; set; } = new(JsonSerializerDefaults.Web);

    private HttpClient GetHttpClient() => _clientFactory.CreateClient(ApiEndpointConstants.CommonApiClientName);


    
}
