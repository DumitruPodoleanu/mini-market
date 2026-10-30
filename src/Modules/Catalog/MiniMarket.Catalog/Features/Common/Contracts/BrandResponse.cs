namespace MiniMarket.Catalog.Features.Common.Contracts;

internal sealed record BrandResponse(
    Guid Id,
    string Name,
    string Slug
);
