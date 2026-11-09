using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;

namespace MiniMarket.Shopping.Features.Wishlist.AddToWishlist;

internal sealed class AddToWishlistEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/api/wishlist/{productSlug}", async (string productSlug, AddToWishlistHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(productSlug, cancellationToken)).ToNoContentHttpResult())
            .WithTags("Wishlist")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesApiErrors(StatusCodes.Status404NotFound, StatusCodes.Status409Conflict);
}
