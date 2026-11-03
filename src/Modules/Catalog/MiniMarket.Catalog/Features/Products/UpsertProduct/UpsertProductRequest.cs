namespace MiniMarket.Catalog.Features.Products.UpsertProduct;

internal sealed record UpsertProductRequest(
    string? Name,
    string? Slug,
    decimal? Price,
    bool? Published,
    int? StockQuantity,
    string? BrandSlug,
    IReadOnlyList<string>? CategorySlugs
);