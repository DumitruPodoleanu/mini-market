using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Shopping.Domain.Entities;

namespace MiniMarket.Shopping.Infrastructure.Configurations;

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable("CartItems");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
    }
}
