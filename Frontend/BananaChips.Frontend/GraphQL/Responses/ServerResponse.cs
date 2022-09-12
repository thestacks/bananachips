using System.Net;
using GraphQL.Client.Http;

namespace BananaChips.Frontend.GraphQL.Responses;

public class ServerResponse<T>
{
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? ValidationErrorCode { get; set; }

    public bool IsSuccess
    {
        get
        {
            var statusCode = (int)StatusCode;
            return statusCode is >= 200 and <= 302;
        }
    }

    public ServerResponse(GraphQLHttpResponse<T> graphQlHttpResponse)
    {
        Data = graphQlHttpResponse.Data;
        StatusCode = graphQlHttpResponse.StatusCode;
        if (graphQlHttpResponse.StatusCode == HttpStatusCode.BadRequest)
        {
            var validationError = graphQlHttpResponse.Errors?.FirstOrDefault(e =>
                e.Extensions.TryGetValue("type", out var type) && (string) type == "validation");
            ValidationErrorCode = validationError?.Extensions?["code"].ToString();
        }
    }
}