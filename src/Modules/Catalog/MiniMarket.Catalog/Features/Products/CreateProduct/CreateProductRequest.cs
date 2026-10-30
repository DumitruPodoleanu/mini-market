using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Products.CreateProduct;

internal sealed record CreateProductRequest(
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    Guid BrandId,
    IReadOnlyList<Guid>? CategoryIds) : IProductRequest;
