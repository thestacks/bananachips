using System.Text.Json.Serialization;

namespace BananaChips.Frontend.GraphQL.Requests;

public interface IGraphQLRequestBase
{
    [JsonIgnore]
    public string Query { get; }
    
    [JsonIgnore]
    public string OperationName { get; }
}