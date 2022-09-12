using System.Text.Json;
using BananaChips.Frontend.Constants;
using BananaChips.Frontend.GraphQL.Responses;
using BananaChips.Frontend.Services.Meta;
using Blazored.LocalStorage;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace BananaChips.Frontend.Services;

public class CompanyClient : IBackendConnector
{
    private readonly ILocalStorageService _localStorage;
    private readonly GraphQLHttpClient _client;

    public CompanyClient(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _client = new GraphQLHttpClient("https://localhost:5000/graphql",
            new SystemTextJsonSerializer(options => options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase));
    }

    public async Task<ServerResponse<TResponse>> SendQueryAsync<TResponse>(string query, string operationName, object variables = null, bool anonymous = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GraphQLRequest(query)
            {
                OperationName = operationName,
                Variables = variables
            };

            _client.HttpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!anonymous)
                _client.HttpClient.DefaultRequestHeaders.Add("Authorization", await GetAccessTokenAsync());

            var response = await _client.SendQueryAsync<TResponse>(request, cancellationToken);
            return new ServerResponse<TResponse>(response.AsGraphQLHttpResponse());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured when sending query. {ex.Message}");
            return null;
        }
    }

    public async Task<ServerResponse<TResponse>> SendMutationAsync<TResponse>(string query, string operationName, object variables = null, bool anonymous = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GraphQLRequest(query)
            {
                Variables = variables,
                OperationName = operationName
            };

            _client.HttpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!anonymous)
                _client.HttpClient.DefaultRequestHeaders.Add("Authorization", await GetAccessTokenAsync());

            var response = await _client.SendMutationAsync<TResponse>(request, cancellationToken);
            return new ServerResponse<TResponse>(response.AsGraphQLHttpResponse());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured when sending mutation. {ex.Message}");
            return null;
        }
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(LocalStorageKeysConstants.Token);
        return $"Bearer {token}";
    }
}