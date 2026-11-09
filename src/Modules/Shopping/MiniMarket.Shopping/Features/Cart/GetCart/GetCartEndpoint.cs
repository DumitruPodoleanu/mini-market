using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;
using MiniMarket.Shopping.Features.Common.Responses;

namespace MiniMarket.Shopping.Features.Cart.GetCart;

internal sealed class GetCartEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/cart", async (GetCartHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(cancellationToken)).ToHttpResult())
            .WithTags("Cart")
            .ProducesApiResponse<CartResponse>();
}
