using Microsoft.Extensions.Logging;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Features.Common;
using MiniMarket.Shopping.Features.Common.Errors;
using MiniMarket.Shopping.Features.Common.Mappings;

namespace MiniMarket.Shopping.Features.Cart.SetCartItem;

internal sealed class SetCartItemHandler(
    ICartRepository cartRepository,
    ProductLookup productLookup,
    ICurrentUser currentUser,
    ILogger<SetCartItemHandler> logger)
{
    public async Task<Result> HandleAsync(SetCartItemCommand command, CancellationToken cancellationToken)
    {
        var result = await productLookup.GetPublishedAsync(command.ProductSlug, cancellationToken);
        if (!result.IsSuccess)
            return result.Error!;

        var product = result.Value!;
        var quantity = command.Body.Quantity!.Value;

        if (quantity > product.StockQuantity)
        {
            var error = ShoppingErrors.NotEnoughStock(command.ProductSlug, product.StockQuantity);
            logger.LogWarning("Cart update failed: {Error}", error.Message);
            return error;
        }

        var updated = await cartRepository.UpdateQuantityAsync(currentUser.Id, product.Id, quantity, cancellationToken);
        if (!updated)
            await cartRepository.AddAsync(product.ToCartItemEntity(currentUser.Id, quantity), cancellationToken);

        logger.LogInformation("Quantity of product {ProductId} set to {Quantity} in the cart of user {UserId}", product.Id, quantity, currentUser.Id);

        return Result.Success();
    }
}
