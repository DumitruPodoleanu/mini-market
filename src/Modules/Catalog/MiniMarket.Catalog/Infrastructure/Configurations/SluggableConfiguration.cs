using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal static class SluggableConfiguration
{
    public static void HasSlug<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : SluggableEntity
    {
        builder.HasOne(e => e.Slug)
            .WithOne()
            .HasForeignKey<TEntity>(e => e.SlugId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}