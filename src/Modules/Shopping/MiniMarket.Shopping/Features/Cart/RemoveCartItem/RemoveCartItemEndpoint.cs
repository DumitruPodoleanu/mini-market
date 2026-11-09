using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;

namespace MiniMarket.Shopping.Features.Cart.RemoveCartItem;

internal sealed class RemoveCartItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/cart/items/{productSlug}", async (string productSlug, RemoveCartItemHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(productSlug, cancellationToken)).ToNoContentHttpResult())
            .WithTags("Cart")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesApiErrors(StatusCodes.Status404NotFound);
}
