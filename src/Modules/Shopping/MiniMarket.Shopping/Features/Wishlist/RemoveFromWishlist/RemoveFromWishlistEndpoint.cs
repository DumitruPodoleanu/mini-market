using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;

namespace MiniMarket.Shopping.Features.Wishlist.RemoveFromWishlist;

internal sealed class RemoveFromWishlistEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/wishlist/{productSlug}", async (string productSlug, RemoveFromWishlistHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(productSlug, cancellationToken)).ToNoContentHttpResult())
            .WithTags("Wishlist")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesApiErrors(StatusCodes.Status404NotFound);
}
