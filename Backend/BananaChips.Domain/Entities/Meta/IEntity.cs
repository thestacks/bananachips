namespace BananaChips.Domain.Entities.Meta;

public interface IEntity
{
    bool Deleted { get; set; }
    long TimeStamp { get; set; }
}

public interface IEntity<out TKey> : IEntity
{
    TKey Id { get; }
}