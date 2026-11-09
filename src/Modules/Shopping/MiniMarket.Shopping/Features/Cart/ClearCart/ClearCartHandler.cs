using Microsoft.Extensions.Logging;
using MiniMarket.Shared.Results;
using MiniMarket.Shared.Users;
using MiniMarket.Shopping.Domain.Repositories;

namespace MiniMarket.Shopping.Features.Cart.ClearCart;

internal sealed class ClearCartHandler(
    ICartRepository cartRepository,
    ICurrentUser currentUser,
    ILogger<ClearCartHandler> logger)
{
    public async Task<Result> HandleAsync(CancellationToken cancellationToken)
    {
        await cartRepository.DeleteAllAsync(currentUser.Id, cancellationToken);

        logger.LogInformation("Cart of user {UserId} cleared", currentUser.Id);

        return Result.Success();
    }
}
