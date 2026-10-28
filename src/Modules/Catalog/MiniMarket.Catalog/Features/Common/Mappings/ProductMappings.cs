using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class ProductMappings
{
    public static void MapTo(this IProductRequest request, Product product, IReadOnlyList<Category> categories)
    {
        product.Sku = request.Sku.Trim().ToUpperInvariant();
        product.Name = request.Name.Trim();
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

    public static Product ToEntity(this IProductRequest request, IReadOnlyList<Category> categories)
    {
        var product = new Product { Id = Guid.CreateVersion7() };
        request.MapTo(product, categories);
        return product;
    }
}