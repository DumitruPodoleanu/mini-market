using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;

namespace MiniMarket.Shopping.Features.Cart.ClearCart;

internal sealed class ClearCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/cart", async (ClearCartHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(cancellationToken)).ToNoContentHttpResult())
            .WithTags("Cart")
            .Produces(StatusCodes.Status204NoContent);
}
