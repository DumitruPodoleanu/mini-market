using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Translations;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku).HasMaxLength(ProductConstraints.SkuMaxLength).IsRequired();
        builder.HasIndex(p => new { p.VendorId, p.Sku }).IsUnique();

        builder.Property(p => p.Name).HasMaxLength(ProductConstraints.NameMaxLength).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);

        builder.HasSlug();

        builder.HasTranslations<ProductEntity, ProductTranslationEntity>(translation =>
            translation.Property(t => t.Name).HasMaxLength(ProductConstraints.NameMaxLength));

        builder.HasOne(p => p.Brand)
            .WithMany()
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Vendor)
            .WithMany()
            .HasForeignKey(p => p.VendorId)
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