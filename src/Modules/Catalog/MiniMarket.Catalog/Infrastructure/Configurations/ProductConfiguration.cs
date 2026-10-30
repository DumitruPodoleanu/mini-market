using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku).HasMaxLength(ProductConstraints.SkuMaxLength).IsRequired();
        builder.HasIndex(p => p.Sku).IsUnique();

        builder.Property(p => p.Name).HasMaxLength(ProductConstraints.NameMaxLength).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);

        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Categories)
            .WithMany()
            .UsingEntity(
                "ProductCategories",
                right => right.HasOne(typeof(CategoryEntity)).WithMany().HasForeignKey("CategoryId"),
                left => left.HasOne(typeof(ProductEntity)).WithMany().HasForeignKey("ProductId"),
                join =>
                {
                    join.HasKey("ProductId", "CategoryId");
                });
    }
}