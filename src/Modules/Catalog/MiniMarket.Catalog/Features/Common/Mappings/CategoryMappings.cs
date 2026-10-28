using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class CategoryMappings
{
    public static CategoryResponse ToResponse(this Category category) => new(
        category.Id,
        category.Name,
        category.Slug
    );
}