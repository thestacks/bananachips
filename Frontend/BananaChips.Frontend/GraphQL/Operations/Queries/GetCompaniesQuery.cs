using System.Text.Json.Serialization;
using BananaChips.Frontend.GraphQL.Requests;
using BananaChips.Frontend.GraphQL.Responses;
using MudBlazor;

namespace BananaChips.Frontend.GraphQL.Operations.Queries;

public class GetCompaniesQuery
{
    public class Request : IGraphQLRequestBase
    {
        public int Skip { get; }
        public int Take { get; }
        private string? SortField { get; }
        private string? SortDirection { get; }
        public string SearchText { get; }

        public Request(int skip, int take, string searchText, string? sortField = null,
            SortDirection? sortDirection = null)
        {
            Skip = skip;
            Take = take;
            SearchText = searchText ?? string.Empty;
            SortField = sortField;
            SortDirection = sortDirection == MudBlazor.SortDirection.None ? null :
                sortDirection == MudBlazor.SortDirection.Ascending ? "ASC" : "DESC";
        }

        [JsonIgnore]
        public string Query
        {
            get
            {
                if (SortField != null && SortDirection != null)
                {
                    return QueryWithSort.Replace("$sortField", SortField.ToLower()).Replace("$sortDirection",
                        SortDirection);
                }

                return QueryWithoutSort;
            }
        }

        [JsonIgnore] public string OperationName => "GetCompanies";

        private string QueryWithoutSort => $@"query {OperationName} ($skip: Int!, $take: Int!, $searchText: String) {{
    companies (skip: $skip, take: $take, where: {{or: [{{identifier: {{contains: $searchText}} }}, {{name: {{contains: $searchText}}}}]}}) {{
        items {{
            id
            identifier
            name
        }}
        totalCount
    }}
}}";

        private string QueryWithSort => $@"query {OperationName} ($skip: Int!, $take: Int!, $searchText: String) {{
    companies (skip: $skip, take: $take, where: {{or: [{{identifier: {{contains: $searchText}} }}, {{name: {{contains: $searchText}}}}]}} order: {{ $sortField: $sortDirection }}) {{
        items {{
            id
            identifier
            name
        }}
        totalCount
    }}
}}";
    }

    public class Response
    {
        public PageResponse<CompanyListElement> Companies { get; set; }

        public class CompanyListElement
        {
            public int Id { get; set; }
            public string Identifier { get; set; }
            public string Name { get; set; }
        }
    }
}