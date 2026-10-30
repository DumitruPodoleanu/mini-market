using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(CategoryConstraints.NameMaxLength).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(CategoryConstraints.SlugMaxLength).IsRequired();
        builder.HasIndex(c => c.Slug).IsUnique();
    }
}