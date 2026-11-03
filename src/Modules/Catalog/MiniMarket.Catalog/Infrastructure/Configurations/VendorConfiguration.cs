using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class VendorConfiguration : IEntityTypeConfiguration<VendorEntity>
{
    public void Configure(EntityTypeBuilder<VendorEntity> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Name).HasMaxLength(VendorConstraints.NameMaxLength).IsRequired();
        builder.HasSlug();
    }
}