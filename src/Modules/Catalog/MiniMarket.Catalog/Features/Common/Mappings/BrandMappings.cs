using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class BrandMappings
{
    public static BrandResponse ToResponse(this Brand brand) => new(
        brand.Id,
        brand.Name,
        brand.Slug
    );
}