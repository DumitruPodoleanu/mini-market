using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Infrastructure.Seed;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(BrandConstraints.NameMaxLength).IsRequired();
        builder.Property(b => b.Slug).HasMaxLength(BrandConstraints.SlugMaxLength).IsRequired();
        builder.HasIndex(b => b.Slug).IsUnique();

        builder.HasData(CatalogSeed.Brands);
    }
}