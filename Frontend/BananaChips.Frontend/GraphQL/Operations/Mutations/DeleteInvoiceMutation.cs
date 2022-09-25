using BananaChips.Frontend.GraphQL.Requests;

namespace BananaChips.Frontend.GraphQL.Operations.Mutations;

public class DeleteInvoiceMutation
{
    public class Request : IGraphQLRequestBase
    {
        public int Id { get; set; }
    
        public Request(int id)
        {
            Id = id;
        }
    
        public string Query => $@"mutation {OperationName}($id: Long!) {{
  deleteInvoice(input: {{ id: $id }}) {{
    success
  }}
}}";

        public string OperationName => "DeleteInvoice";
    }
    
    public class Response
    {
        public bool Success { get; set; }
        
        public class Wrapper
        {
            public Response DeleteInvoice { get; set; }
        }
    }
}