using BananaChips.Frontend.GraphQL.Requests;
using BananaChips.Frontend.GraphQL.Responses;

namespace BananaChips.Frontend.GraphQL.Operations.Queries;

public class GetCompaniesLookupQuery
{
    public class Request : IGraphQLRequestBase
    {
        public string SearchText { get; set; }

        public Request(string searchText)
        {
            SearchText = searchText;
        }

        public string Query => $@"query {OperationName} ($searchText: String) {{
            companies (where: {{ identifier {{ contains: $searchText }} }}) {{
                id
                identifier
                name
            }}
        }}";
        public string OperationName => "GetCompaniesLookup";
    }

    public class Response
    {
        public PageResponse<CompanyLookupElement> Companies { get; set; }
        public class CompanyLookupElement
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Identifier { get; set; }
        }
    }
}