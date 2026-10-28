using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Infrastructure.Seed;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
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
                right => right.HasOne(typeof(Category)).WithMany().HasForeignKey("CategoryId"),
                left => left.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId"),
                join =>
                {
                    join.HasKey("ProductId", "CategoryId");
                    join.HasData(CatalogSeed.ProductCategories);
                });

        builder.HasData(CatalogSeed.Products);
    }
}