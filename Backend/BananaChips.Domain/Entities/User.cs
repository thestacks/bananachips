using BananaChips.Domain.Entities.Meta;
using Microsoft.AspNetCore.Identity;

namespace BananaChips.Domain.Entities;

public class User : IdentityUser, IEntity<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; private set; }
    public bool Deleted { get; set; }
    public long TimeStamp { get; set; }
}