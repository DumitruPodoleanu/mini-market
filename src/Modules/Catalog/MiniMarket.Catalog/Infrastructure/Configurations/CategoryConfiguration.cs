using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Infrastructure.Seed;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Slug).HasMaxLength(100).IsRequired();
        builder.HasIndex(b => b.Slug).IsUnique();

        builder.HasData(CatalogSeed.Categories);
    }
}