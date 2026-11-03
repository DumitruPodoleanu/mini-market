using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Localization;
using MiniMarket.Catalog.Features.Products.UpsertProduct;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class ProductMappings
{
    public static ProductEntity ToEntity(
        this UpsertProductRequest request,
        string sku,
        string slug,
        VendorEntity vendor,
        BrandEntity brand,
        IReadOnlyList<CategoryEntity> categories) => new()
        {
            Id = Guid.CreateVersion7(),
            Sku = sku,
            Name = NormalizeName(request.Name!),
            Slug = new SlugEntity { Id = Guid.CreateVersion7(), Value = slug, EntityType = SlugEntityType.Product },
            Price = request.Price!.Value,
            Published = request.Published ?? false,
            StockQuantity = request.StockQuantity ?? 0,
            Vendor = vendor,
            Brand = brand,
            Categories = [.. categories]
        };

    public static void ApplyTo(
        this UpsertProductRequest request,
        ProductEntity product,
        BrandEntity? brand,
        IReadOnlyList<CategoryEntity>? categories,
        string? slug)
    {
        if (request.Name is not null)
            product.Name = NormalizeName(request.Name);

        if (request.Price is not null)
            product.Price = request.Price.Value;

        if (request.Published is not null)
            product.Published = request.Published.Value;

        if (request.StockQuantity is not null)
            product.StockQuantity = request.StockQuantity.Value;

        if (brand is not null)
            product.Brand = brand;

        if (categories is not null)
        {
            product.Categories.Clear();
            foreach (var category in categories)
            {
                product.Categories.Add(category);
            }
        }

        if (slug is not null)
            product.Slug!.Value = slug;
    }

    public static ProductResponse ToResponse(this ProductEntity product, string language) => new(
        product.Sku,
        product.Translate(language, t => t.Name, product.Name),
        product.Slug!.Value,
        product.Price,
        product.Published,
        product.StockQuantity,
        product.Brand!.ToResponse(),
        product.Vendor!.ToResponse(),
        product.Categories.Select(c => c.ToResponse(language)).ToList()
    );

    public static string NormalizeSku(string sku) => sku.Trim().ToUpperInvariant();

    private static string NormalizeName(string name) => name.Trim();
}