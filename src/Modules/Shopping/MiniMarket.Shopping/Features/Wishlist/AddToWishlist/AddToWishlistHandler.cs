using Microsoft.Extensions.Logging;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Common.Mappings;

namespace MiniMarket.Shopping.Features.Wishlist.AddToWishlist;

internal sealed class AddToWishlistHandler(
    IWishlistRepository wishlistRepository,
    ProductLookup productLookup,
    ICurrentUser currentUser,
    ILogger<AddToWishlistHandler> logger)
{
    public async Task<Result> HandleAsync(string productSlug, CancellationToken cancellationToken)
    {
        var product = await productLookup.GetPublishedAsync(productSlug, cancellationToken);
        if (!product.IsSuccess)
            return product.Error!;

        var productId = product.Value!.Id;

        var existing = await wishlistRepository.GetAsync(currentUser.Id, productId, cancellationToken);
        if (existing is not null)
            return Result.Success();

        await wishlistRepository.AddAsync(product.Value.ToWishlistItemEntity(currentUser.Id), cancellationToken);

        logger.LogInformation("Product {ProductId} added to the wishlist of user {UserId}", productId, currentUser.Id);

        return Result.Success();
    }
}
