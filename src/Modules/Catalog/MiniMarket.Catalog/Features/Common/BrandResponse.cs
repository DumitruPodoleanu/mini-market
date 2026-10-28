namespace MiniMarket.Catalog.Features.Common;

internal sealed record BrandResponse(
    Guid Id,
    string Name,
    string Slug
);
