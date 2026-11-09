using MiniMarket.Shared.Results;

namespace MiniMarket.Shopping.Features.Common.Errors;

internal static class ShoppingErrors
{
    public static Error ProductNotFound(string productSlug) =>
        new($"Product with slug '{productSlug}' was not found.", ErrorType.NotFound);

    public static Error ProductNotPublished(string productSlug) =>
        new($"Product with slug '{productSlug}' is not available.", ErrorType.Conflict);

    public static Error NotInWishlist(string productSlug) =>
        new($"Product with slug '{productSlug}' is not in the wishlist.", ErrorType.NotFound);

    public static Error NotInCart(string productSlug) =>
        new($"Product with slug '{productSlug}' is not in the cart.", ErrorType.NotFound);

    public static Error NotEnoughStock(string productSlug, int stockQuantity) =>
        new($"Only {stockQuantity} of product '{productSlug}' are in stock.", ErrorType.Conflict);
}