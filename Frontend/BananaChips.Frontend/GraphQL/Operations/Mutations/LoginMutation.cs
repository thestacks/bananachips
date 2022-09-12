using System.Text.Json.Serialization;
using BananaChips.Frontend.GraphQL.Requests;

namespace BananaChips.Frontend.GraphQL.Operations.Mutations;

public class LoginMutation
{
    public class Request : IGraphQLRequestBase
    {
        public Request(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public string Query => $@"mutation {OperationName}($email: String!, $password: String!){{
                                login (input: {{ email: $email, password: $password }}) {{
                                    accessToken
                                }}
                            }}";
        [JsonIgnore]
        public string OperationName => "Login";
    }

    public class Response
    {
        public string AccessToken { get; set; }
        
        public class Wrapper
        {
            public Response Login { get; set; }
        }
    }
}