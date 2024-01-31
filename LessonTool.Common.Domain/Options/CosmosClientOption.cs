namespace LessonTool.Common.Domain.Options;

public class CosmosClientOption
{
    public string Endpoint { get; set; } = string.Empty;
    public string AccountKey { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
