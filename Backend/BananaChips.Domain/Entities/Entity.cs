using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BananaChips.Domain.Entities.Meta;

namespace BananaChips.Domain.Entities;

public class Entity<TKey> : IEntity<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; }

    public bool Deleted { get; set; }
    public long TimeStamp { get; set; }
}