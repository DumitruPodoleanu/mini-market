using Microsoft.AspNetCore.Mvc;

namespace MiniMarket.Shopping.Features.Cart.SetCartItem;

internal sealed record SetCartItemCommand(
    string ProductSlug,
    [FromBody] SetCartItemRequest Body);
