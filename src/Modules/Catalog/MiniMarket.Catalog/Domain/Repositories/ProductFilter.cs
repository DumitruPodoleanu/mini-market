namespace MiniMarket.Catalog.Domain.Repositories;

internal sealed record ProductFilter(string? CategorySlug, string? Name, bool? Published, decimal? MinPrice, decimal? MaxPrice, string? BrandSlug, ProductSort Sort, int Page, int PageSize);