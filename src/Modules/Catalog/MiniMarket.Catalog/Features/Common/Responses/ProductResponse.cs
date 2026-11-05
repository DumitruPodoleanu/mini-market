namespace MiniMarket.Catalog.Features.Common.Responses;

internal sealed record ProductResponse(
    string Sku,
    string Name,
    string Slug,
    decimal Price,
    bool Published,
    int StockQuantity,
    BrandResponse Brand,
    VendorResponse Vendor,
    IReadOnlyList<CategoryResponse> Categories);
