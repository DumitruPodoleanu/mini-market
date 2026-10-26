using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common;

internal sealed record BrandResponse(
    Guid Id,
    string Name,
    string Slug
)
{
    public static BrandResponse FromDomain(Brand brand) =>
        new(
            brand.Id,
            brand.Name,
            brand.Slug
            );
}
