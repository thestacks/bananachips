using BananaChips.Frontend.GraphQL.Responses;

namespace BananaChips.Frontend.Services.Meta;

public interface IBackendConnector
{
    Task<ServerResponse<TResponse>> SendQueryAsync<TResponse>(string query, string operationName,
        object variables = null, bool anonymous = false,
        CancellationToken cancellationToken = default);

    Task<ServerResponse<TResponse>> SendMutationAsync<TResponse>(string query, string operationName,
        object variables = null, bool anonymous = false,
        CancellationToken cancellationToken = default);
}