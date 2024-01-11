using Microsoft.Azure.Cosmos;

namespace LessonTool.API.Infrastructure.Interfaces
{
    public interface ICosmosContainerFactory
    {
        Container CreateDataContainer();
    }
}