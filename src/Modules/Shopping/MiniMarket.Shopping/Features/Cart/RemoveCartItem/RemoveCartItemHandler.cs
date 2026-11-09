using Microsoft.Extensions.Logging;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Common.Errors;

namespace MiniMarket.Shopping.Features.Cart.RemoveCartItem;

internal sealed class RemoveCartItemHandler(
    ICartRepository cartRepository,
    ProductLookup productLookup,
    ICurrentUser currentUser,
    ILogger<RemoveCartItemHandler> logger)
{
    public async Task<Result> HandleAsync(string productSlug, CancellationToken cancellationToken)
    {
        var product = await productLookup.GetAsync(productSlug, cancellationToken);
        if (!product.IsSuccess)
            return product.Error!;

        var productId = product.Value!.Id;

        var deleted = await cartRepository.DeleteAsync(currentUser.Id, productId, cancellationToken);
        if (!deleted)
        {
            var error = ShoppingErrors.NotInCart(productSlug);
            logger.LogWarning("Cart remove failed: {Error}", error.Message);
            return error;
        }

        logger.LogInformation("Product {ProductId} removed from the cart of user {UserId}", productId, currentUser.Id);

        return Result.Success();
    }
}
