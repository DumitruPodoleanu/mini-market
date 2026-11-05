using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Shopping.Domain.Entities;

namespace MiniMarket.Shopping.Infrastructure.Configurations;

internal sealed class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItemEntity>
{
    public void Configure(EntityTypeBuilder<WishlistItemEntity> builder)
    {
        builder.ToTable("WishlistItems");
        builder.HasKey(w => w.Id);
        builder.HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();
    }
}
