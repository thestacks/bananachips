using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace BananaChips.Domain.Entities;

public class Address : Entity<int>
{
    public string Line { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    [GraphQLIgnore]
    [ForeignKey(nameof(Company))]
    public int CompanyId { get; set; }
    [GraphQLIgnore]
    public Company Company { get; set; }
}