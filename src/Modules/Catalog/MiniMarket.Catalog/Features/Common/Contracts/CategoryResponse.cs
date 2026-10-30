namespace MiniMarket.Catalog.Features.Common.Contracts;

internal sealed record CategoryResponse(
    Guid Id,
    string Name,
    string Slug
);
