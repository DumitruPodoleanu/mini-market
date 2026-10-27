using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal sealed record UpdateProductRequest(
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    Guid BrandId,
    IReadOnlyList<Guid>? CategoryIds) : IProductRequest;
