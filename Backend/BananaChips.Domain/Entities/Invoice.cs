using System.ComponentModel.DataAnnotations.Schema;
using BananaChips.Domain.Enums;
using HotChocolate;

namespace BananaChips.Domain.Entities;

public class Invoice : Entity<long>
{
    public string Name { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public InvoiceType Type { get; set; }
    public decimal NetValue { get; set; }
    public decimal GrossValue { get; set; }
    public ICollection<InvoiceLine> Lines { get; set; }

    [GraphQLIgnore]
    [ForeignKey(nameof(Seller))]
    public int SellerId { get; set; }
    public Company Seller { get; set; }
    
    [GraphQLIgnore]
    [ForeignKey(nameof(Buyer))]
    public int BuyerId { get; set; }
    public Company Buyer { get; set; }
}