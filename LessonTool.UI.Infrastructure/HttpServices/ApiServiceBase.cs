using LessonTool.Common.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using static LessonTool.Common.Domain.Utilities.HttpUtilities;

namespace LessonTool.UI.Infrastructure.HttpServices;

public abstract class ApiServiceBase<T> : IRepository<T> where T : class
{
    private readonly string _httpClientName;
    private readonly string _apiEndpoint;

    private readonly IHttpClientFactory _clientFactory;

    protected JsonSerializerOptions jsonOptions { get; set; } = new(JsonSerializerDefaults.Web);


    public ApiServiceBase(IServiceProvider serviceProvider, string httpClientName, string endpoint)
    {
        _httpClientName = httpClientName;
        _apiEndpoint = endpoint;

        _clientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    }

    /// <summary>
    /// Provies a HttpClient client for the class to use
    /// </summary>
    /// <returns>HttpClient</returns>
    protected virtual Task<HttpClient> GetClient()
    {
        var client = _clientFactory.CreateClient(_httpClientName);
        return Task.FromResult(client);
    }


    public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.GetAsync(_apiEndpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await DeserializeResponse<List<T>>(response, jsonOptions, cancellationToken);
    }

    public virtual async Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.GetAsync(BuildQueryString(_apiEndpoint, new() { { nameof(id), id.ToString() } }), cancellationToken);
        response.EnsureSuccessStatusCode();

        return await DeserializeResponse<T>(response, jsonOptions, cancellationToken);
    }

    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.PostAsync(_apiEndpoint, GeneratePayload(entity, jsonOptions), cancellationToken);
        response.EnsureSuccessStatusCode();

        return await DeserializeResponse<T>(response, jsonOptions, cancellationToken);
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.PutAsync(_apiEndpoint, GeneratePayload(entity, jsonOptions), cancellationToken);
        response.EnsureSuccessStatusCode();

        return await DeserializeResponse<T>(response, jsonOptions, cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var client = await GetClient();

        var response = await client.DeleteAsync(BuildQueryString(_apiEndpoint, new() { { nameof(id), id.ToString() } }), cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
