using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;
using MiniMarket.Infrastructure.Validation;

namespace MiniMarket.Shopping.Features.Cart.SetCartItem;

internal sealed class SetCartItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/api/cart/items/{productSlug}", async ([AsParameters] SetCartItemCommand command, SetCartItemHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(command, cancellationToken)).ToNoContentHttpResult())
            .AddEndpointFilter<ValidationFilter<SetCartItemCommand>>()
            .WithTags("Cart")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesApiErrors(StatusCodes.Status400BadRequest, StatusCodes.Status404NotFound, StatusCodes.Status409Conflict);
}
