using Microsoft.Extensions.Logging;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Common.Errors;

namespace MiniMarket.Shopping.Features.Wishlist.RemoveFromWishlist;

internal sealed class RemoveFromWishlistHandler(
    IWishlistRepository wishlistRepository,
    ProductLookup productLookup,
    ICurrentUser currentUser,
    ILogger<RemoveFromWishlistHandler> logger)
{
    public async Task<Result> HandleAsync(string productSlug, CancellationToken cancellationToken)
    {
        var product = await productLookup.GetAsync(productSlug, cancellationToken);
        if (!product.IsSuccess)
            return product.Error!;

        var productId = product.Value!.Id;

        var deleted = await wishlistRepository.DeleteAsync(currentUser.Id, productId, cancellationToken);
        if (!deleted)
        {
            var error = ShoppingErrors.NotInWishlist(productSlug);
            logger.LogWarning("Wishlist remove failed: {Error}", error.Message);
            return error;
        }

        logger.LogInformation("Product {ProductId} removed from the wishlist of user {UserId}", productId, currentUser.Id);

        return Result.Success();
    }
}
