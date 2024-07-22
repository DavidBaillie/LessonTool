using LessonTool.API.Endpoint.Test.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LessonTool.API.Endpoint.Test.Configuration;

internal class EndpointTestBase
{
    private readonly WebApplicationFactory<ITestMarker> _factory;

    protected HttpClient Client { get; }

    public EndpointTestBase(TestIngOptions options)
    {
        _factory = new EndpointWebApplicationFactory().CreateFromTestOptions(options);
        Client = _factory.CreateClient();
    }

    protected IServiceScope CreateTestScope()
    {
        return _factory.Services.CreateScope();
    }
}
