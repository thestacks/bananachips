using BananaChips.Frontend.Components.Company;
using BananaChips.Frontend.GraphQL.Requests;

namespace BananaChips.Frontend.GraphQL.Operations.Mutations;

public class UpsertCompanyMutation
{
    public class Request : IGraphQLRequestBase
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressCountry { get; set; }
    
        public Request(CompanyForm.CompanyFormModel form)
        {
            Id = form.Id;
            Name = form.Name;
            Identifier = form.Identifier;
            AddressLine = form.AddressLine;
            AddressCity = form.AddressCity;
            AddressZipCode = form.AddressZipCode;
            AddressCountry = form.AddressCountry;
        }
    
        public string Query => $@"mutation {OperationName}($id: Int, $name: String!, $identifier: String!, $addressLine: String!, $addressCity: String!, $addressZipCode: String!, $addressCountry: String!) {{
  upsertCompany(input: {{ id: $id, name: $name, identifier: $identifier, address: {{ line: $addressLine, city: $addressCity, zipCode: $addressZipCode, country: $addressCountry}} }}) {{
    id
  }}
}}";

        public string OperationName => "CreateCompany";
    }
    
    public class Response
    {
        public int Id { get; set; }
        
        public class Wrapper
        {
            public Response UpsertCompany { get; set; }
        }
    }
}