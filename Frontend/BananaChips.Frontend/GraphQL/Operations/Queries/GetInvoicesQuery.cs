using System.Text.Json.Serialization;
using BananaChips.Frontend.GraphQL.Requests;
using BananaChips.Frontend.GraphQL.Responses;
using MudBlazor;

namespace BananaChips.Frontend.GraphQL.Operations.Queries;

public class GetInvoicesQuery
{
    public class Request : IGraphQLRequestBase
    {
        private static readonly List<string> NestedSortFields = new() { "seller", "buyer" }; 
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
            SortField = sortField?.ToLower();
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
                    if (!NestedSortFields.Contains(SortField))
                        return QueryWithSort.Replace("$sortField", SortField.ToLower()).Replace("$sortDirection",
                            SortDirection);

                    return QueryWithSort.Replace("$sortField: $sortDirection",
                        $"{SortField}: {{ name: {SortDirection} }}");
                }

                return QueryWithoutSort;
            }
        }

        [JsonIgnore] public string OperationName => "GetInvoices";

        private string QueryWithoutSort => $@"query {OperationName} ($skip: Int!, $take: Int!, $searchText: String) {{
    invoices (skip: $skip, take: $take, where: {{or: [{{name: {{contains: $searchText}} }}]}}) {{
        items {{
            id
            name
            publishDate
            netValue
            grossValue
            seller {{ 
                name
            }}
            buyer {{
                name    
            }}
        }}
        totalCount
    }}
}}";

        private string QueryWithSort => $@"query {OperationName} ($skip: Int!, $take: Int!, $searchText: String) {{
    invoices (skip: $skip, take: $take, where: {{or: [{{name: {{contains: $searchText}} }}]}} order: {{ $sortField: $sortDirection }}) {{
        items {{
            id
            name
            publishDate
            netValue
            grossValue
            seller {{ 
                name
            }}
            buyer {{
                name    
            }}
        }}
        totalCount
    }}
}}";
    }

    public class Response
    {
        public PageResponse<InvoiceListElement> Invoices { get; set; }

        public class InvoiceListElement
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime PublishDate { get; set; }
            public DateTime PaymentDate { get; set; }
            public decimal NetValue { get; set; }
            public decimal GrossValue { get; set; }
            public InvoiceListElementCompany Seller { get; set; }
            public InvoiceListElementCompany Buyer { get; set; }
        }

        public class InvoiceListElementCompany
        {
            public string Name { get; set; }
        }
    }
}