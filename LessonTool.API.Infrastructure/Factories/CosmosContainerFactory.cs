using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.API.Infrastructure.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace LessonTool.API.Infrastructure.Factories;

public class CosmosContainerFactory : ICosmosContainerFactory, IDisposable
{
    private readonly CosmosClientOption _options;
    private readonly CosmosClient _client;

    public CosmosContainerFactory(IOptions<CosmosClientOption> option)
    {
        _options = option.Value;
        _client = new CosmosClient(_options.Endpoint, _options.AccountKey);
    }

    /// <summary>
    /// Called when the factory is disposed
    /// </summary>
    public void Dispose()
    {
        _client.Dispose();
    }

    /// <summary>
    /// Creates a cosmos container based on the registered options
    /// </summary>
    public Container CreateDataContainer()
    {
        return _client.GetContainer(_options.DatabaseName, _options.ContainerName);
    }
}
