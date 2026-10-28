namespace MiniMarket.Catalog.Features.Common;

internal sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    BrandResponse Brand,
    IReadOnlyList<CategoryResponse> Categories);
