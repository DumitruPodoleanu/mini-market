using MiniMarket.Catalog.Domain.Entities.Common;
using MiniMarket.Catalog.Domain.Entities.Translations;

namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class CategoryEntity : TranslatableEntity<CategoryTranslationEntity>
{
    public required string Name { get; set; }
}