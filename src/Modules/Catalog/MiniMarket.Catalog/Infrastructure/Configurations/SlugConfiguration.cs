using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities.Slugs;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class SlugConfiguration : IEntityTypeConfiguration<SlugEntity>
{
    public void Configure(EntityTypeBuilder<SlugEntity> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Value).HasMaxLength(SlugConstraints.SlugMaxLength).IsRequired();
        builder.HasIndex(s => s.Value).IsUnique();
        builder.Property(s => s.EntityType).HasConversion<string>().HasMaxLength(20);
    }
}