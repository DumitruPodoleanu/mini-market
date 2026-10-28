using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal static class ProductErrors
{
    public static Error NotFound(Guid id) =>
        new($"Product {id} was not found.", ErrorType.NotFound);

    public static Error SkuAlreadyExists(string sku) =>
        new($"A product with SKU {sku} already exists.", ErrorType.Conflict);

    public static Error BrandNotFound(Guid brandId) =>
        new($"Brand {brandId} does not exist.", ErrorType.Validation);

    public static Error CategoriesNotFound(IEnumerable<Guid> categoryIds) =>
        new($"Categories do not exist: {string.Join(", ", categoryIds)}.", ErrorType.Validation);
}