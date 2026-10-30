using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class ProductMappings
{
    public static void MapTo(this IProductRequest request, ProductEntity product, IReadOnlyList<CategoryEntity> categories)
    {
        product.Sku = NormalizeSku(request.Sku);
        product.Name = NormalizeName(request.Name);
        product.Price = request.Price;
        product.Published = request.Published;
        product.StockQuantity = request.StockQuantity;
        product.BrandId = request.BrandId;

        product.Categories.Clear();
        foreach (var category in categories)
        {
            product.Categories.Add(category);
        }
    }

    public static ProductEntity ToEntity(this IProductRequest request, IReadOnlyList<CategoryEntity> categories) => new()
    {
        Id = Guid.CreateVersion7(),
        Sku = NormalizeSku(request.Sku),
        Name = NormalizeName(request.Name),
        Price = request.Price,
        Published = request.Published,
        StockQuantity = request.StockQuantity,
        BrandId = request.BrandId,
        Categories = [.. categories]
    };

    public static ProductResponse ToResponse(this ProductEntity product) => new(
        product.Id,
        product.Sku,
        product.Name,
        product.Price,
        product.Published,
        product.StockQuantity,
        product.Brand!.ToResponse(),
        product.Categories.Select(c => c.ToResponse()).ToList()
    );

    private static string NormalizeSku(string sku) => sku.Trim().ToUpperInvariant();

    private static string NormalizeName(string name) => name.Trim();
}