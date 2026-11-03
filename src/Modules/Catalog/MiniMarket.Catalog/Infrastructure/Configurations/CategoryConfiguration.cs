using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Translations;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(CategoryConstraints.NameMaxLength).IsRequired();
        builder.HasSlug();
        builder.HasTranslations<CategoryEntity, CategoryTranslationEntity>(translation =>
            translation.Property(t => t.Name).HasMaxLength(CategoryConstraints.NameMaxLength));
    }
}