using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Products.UpdateProduct;

internal sealed record UpdateProductRequest(
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    Guid BrandId,
    IReadOnlyList<Guid>? CategoryIds) : IProductRequest;
