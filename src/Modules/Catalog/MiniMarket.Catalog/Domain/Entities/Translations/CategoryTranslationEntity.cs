using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Domain.Entities.Translations;

internal sealed class CategoryTranslationEntity : TranslationEntity
{
    public string? Name { get; set; }
}