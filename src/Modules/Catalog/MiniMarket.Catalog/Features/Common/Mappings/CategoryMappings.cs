using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class CategoryMappings
{
    public static CategoryResponse ToResponse(this CategoryEntity category) => new(
        category.Id,
        category.Name,
        category.Slug
    );
}