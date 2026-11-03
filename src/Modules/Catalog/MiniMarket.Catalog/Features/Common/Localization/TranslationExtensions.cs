using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Features.Common.Localization;

internal static class TranslationExtensions
{
    public static string Translate<TTranslation>(
        this TranslatableEntity<TTranslation> entity,
        string language,
        Func<TTranslation, string?> field,
        string fallback)
        where TTranslation : TranslationEntity =>
        entity.Translations.FirstOrDefault(t => t.Language == language) is { } translation
            ? field(translation) ?? fallback
            : fallback;
}
