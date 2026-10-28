namespace MiniMarket.Catalog.Features.Common;

internal sealed record CategoryResponse(
    Guid Id,
    string Name,
    string Slug
);
