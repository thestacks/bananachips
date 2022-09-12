using System.ComponentModel.DataAnnotations.Schema;
using BananaChips.Domain.Enums;
using HotChocolate;

namespace BananaChips.Domain.Entities;

public class InvoiceLine : Entity<long>
{
    public string Name { get; set; }
    public double Amount { get; set; }
    public InvoiceLineUnitType UnitType { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
    public int TaxPercentage { get; set; }
    
    [GraphQLIgnore]
    [ForeignKey(nameof(Invoice))]
    public long InvoiceId { get; set; }
    [GraphQLIgnore]
    public Invoice Invoice { get; set; }
}