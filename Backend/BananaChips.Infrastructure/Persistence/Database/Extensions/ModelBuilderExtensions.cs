using System.Linq.Expressions;
using BananaChips.Domain.Entities.Meta;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Infrastructure.Persistence.Database.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddSoftDeleteQueryFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(IEntity).IsAssignableFrom(e.ClrType)))
        {
            var expressionParameter = Expression.Parameter(entityType.ClrType, "e");
            var expressionProperty = Expression.PropertyOrField(expressionParameter, nameof(IEntity.Deleted));
            var expression = Expression.Lambda(Expression.Equal(expressionProperty, Expression.Constant(false)),
                expressionParameter);
            entityType.SetQueryFilter(expression);
        }

        return modelBuilder;
    }
}