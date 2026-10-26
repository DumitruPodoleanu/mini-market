using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common;

internal sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    bool Published,
    int StockQuantity,
    BrandResponse Brand,
    IReadOnlyList<CategoryResponse> Categories
)
{
    public static ProductResponse FromDomain(Product product) =>
        new(
            product.Id,
            product.Sku,
            product.Name,
            product.Price,
            product.Published,
            product.StockQuantity,
            BrandResponse.FromDomain(product.Brand!),
            product.Categories.Select(CategoryResponse.FromDomain).ToList()
            );
}