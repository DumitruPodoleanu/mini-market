using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal sealed record CreateProductRequest(
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    Guid BrandId,
    IReadOnlyList<Guid>? CategoryIds) : IProductRequest;
