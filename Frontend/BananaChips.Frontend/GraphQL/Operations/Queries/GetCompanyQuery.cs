using BananaChips.Frontend.GraphQL.Requests;
using BananaChips.Frontend.GraphQL.Responses;

namespace BananaChips.Frontend.GraphQL.Operations.Queries;

public class GetCompanyQuery
{
    public class Request : IGraphQLRequestBase
    {
        public int Id { get; set; }

        public Request(int id)
        {
            Id = id;
        }

        public string Query => $@"query {OperationName} ($id: Int!) {{
    companies (where: {{id: {{ eq: $id }}}}) {{
        items {{
            id
            identifier
            name
            address {{
                line
                city
                zipCode
                country
            }}
        }}
    }}
}}";
        public string OperationName => "GetCompany";
    }

    public class Response
    {
        public PageResponse<Company> Companies { get; set; }
        public class Company
        {
            public int Id { get; set; }
            public string Identifier { get; set; }
            public string Name { get; set; }
            public Address Address { get; set; }
        }

        public class Address
        {
            public string Line { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
        }
    }
}