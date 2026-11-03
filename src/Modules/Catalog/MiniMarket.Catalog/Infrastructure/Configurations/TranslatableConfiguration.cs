using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Infrastructure.Configurations;

internal static class TranslatableConfiguration
{
    public static void HasTranslations<TEntity, TTranslation>(
        this EntityTypeBuilder<TEntity> builder,
        Action<OwnedNavigationBuilder<TEntity, TTranslation>> configureFields)
        where TEntity : TranslatableEntity<TTranslation>
        where TTranslation : TranslationEntity
    {
        var owner = typeof(TEntity).Name[..^"Entity".Length];

        builder.OwnsMany(e => e.Translations, translation =>
        {
            translation.ToTable($"{owner}Translations");
            translation.WithOwner().HasForeignKey($"{owner}Id");
            translation.HasKey($"{owner}Id", nameof(TranslationEntity.Language));
            translation.Property(t => t.Language).HasMaxLength(LanguageConstraints.CodeLength);
            configureFields(translation);
        });
    }
}