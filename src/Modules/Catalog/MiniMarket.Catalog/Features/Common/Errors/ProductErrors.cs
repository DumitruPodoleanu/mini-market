using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common.Errors;

internal static class ProductErrors
{
    public static Error NotFound(string productSlug) =>
        new($"Product with slug '{productSlug}' was not found.", ErrorType.NotFound);

    public static Error BrandNotFound(string brandSlug) =>
        new($"Brand with slug '{brandSlug}' does not exist.", ErrorType.Validation);

    public static Error CategoriesNotFound(IEnumerable<string> categorySlugs) =>
        new($"Categories do not exist: {string.Join(", ", categorySlugs)}.", ErrorType.Validation);

    public static Error VendorNotFound(string vendorSlug) =>
        new($"Vendor with slug '{vendorSlug}' does not exist.", ErrorType.NotFound);

    public static Error MissingFieldsForCreate(IEnumerable<string> fields) =>
        new($"Missing fields for a new product: {string.Join(", ", fields)}.", ErrorType.Validation);
}
