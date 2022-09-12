namespace BananaChips.Domain.Entities;

public class Company : Entity<int>
{
    public string Identifier { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
}