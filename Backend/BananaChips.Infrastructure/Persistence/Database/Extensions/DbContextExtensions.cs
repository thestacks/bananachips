using BananaChips.Domain.Entities.Meta;
using BananaChips.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Infrastructure.Persistence.Database.Extensions;

public static class DbContextExtensions
{
    public static void SetModifiedEntitiesTimestamp(this DbContext dbContext)
    {
        var currentTimestamp = DateTime.UtcNow.ToUnixTimeStampMilliseconds();
        foreach (var changedEntity in dbContext.ChangeTracker.Entries().Where(e =>
                     e.Entity is IEntity && e.State is EntityState.Added or EntityState.Modified))
            ((IEntity) changedEntity.Entity).TimeStamp = currentTimestamp;
    }
}