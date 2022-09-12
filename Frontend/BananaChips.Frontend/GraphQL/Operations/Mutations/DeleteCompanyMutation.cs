using BananaChips.Frontend.Components.Company;
using BananaChips.Frontend.GraphQL.Requests;

namespace BananaChips.Frontend.GraphQL.Operations.Mutations;

public class DeleteCompanyMutation
{
    public class Request : IGraphQLRequestBase
    {
        public int Id { get; set; }
    
        public Request(int id)
        {
            Id = id;
        }
    
        public string Query => $@"mutation {OperationName}($id: Int!) {{
  deleteCompany(input: {{ id: $id }}) {{
    success
  }}
}}";

        public string OperationName => "DeleteCompany";
    }
    
    public class Response
    {
        public bool Success { get; set; }
        
        public class Wrapper
        {
            public Response DeleteCompany { get; set; }
        }
    }
}