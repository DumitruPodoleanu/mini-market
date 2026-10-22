using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.GetProduct;

internal sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    Guid BrandId)
{
    public static ProductResponse FromDomain(Product product) =>
        new(
            product.Id,
            product.Sku,
            product.Name,
            product.Price,
            product.Published,
            product.StockQuantity,
            product.BrandId);
}