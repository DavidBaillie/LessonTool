using LessonTool.API.Infrastructure.Interfaces;
using Microsoft.Azure.Cosmos;

namespace LessonTool.API.Infrastructure.Repositories;

public abstract class CosmosRepositoryBase
{
    public virtual async Task<List<T>> ReadCosmosIterator<T>(ICosmosContainerFactory containerFactory, string query, CancellationToken cancellationToken = default)
        where T : class
    {
        var feedIterator = containerFactory
            .CreateDataContainer()
            .GetItemQueryIterator<T>(new QueryDefinition(query));

        var items = new List<T>();
        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync(cancellationToken);
            items.AddRange(response.Resource);
        }

        return items;
    }
}
