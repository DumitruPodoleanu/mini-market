using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Responses;
using MiniMarket.Catalog.Features.Common.Localization;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class CategoryMappings
{
    public static CategoryResponse ToResponse(this CategoryEntity category, string language) => new(
        category.Translate(language, t => t.Name, category.Name),
        category.Slug!.Value
    );
}
