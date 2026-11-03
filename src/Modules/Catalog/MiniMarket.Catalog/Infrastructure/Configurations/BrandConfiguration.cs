using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class BrandConfiguration : IEntityTypeConfiguration<BrandEntity>
{
    public void Configure(EntityTypeBuilder<BrandEntity> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(BrandConstraints.NameMaxLength).IsRequired();
        builder.HasSlug();
    }
}