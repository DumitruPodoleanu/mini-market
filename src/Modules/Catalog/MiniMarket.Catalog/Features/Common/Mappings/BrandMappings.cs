using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Responses;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class BrandMappings
{
    public static BrandResponse ToResponse(this BrandEntity brand) => new(
        brand.Name,
        brand.Slug!.Value
    );
}