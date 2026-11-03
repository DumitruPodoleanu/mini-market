namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed record ListProductsQuery(string? CategorySlug, string? Name, bool? Published, decimal? MinPrice, decimal? MaxPrice, string? BrandSlug, string? Sort = null, int Page = 1, int PageSize = 20)
{
    public const int MaxPageSize = 100;
}