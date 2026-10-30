using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class BrandMappings
{
    public static BrandResponse ToResponse(this BrandEntity brand) => new(
        brand.Id,
        brand.Name,
        brand.Slug
    );
}