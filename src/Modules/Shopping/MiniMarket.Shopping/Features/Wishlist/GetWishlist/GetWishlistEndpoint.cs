using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;
using MiniMarket.Infrastructure.Validation;
using MiniMarket.Shopping.Features.Common.Responses;
using MiniMarket.Shared.Results;

namespace MiniMarket.Shopping.Features.Wishlist.GetWishlist;

internal sealed class GetWishlistEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/wishlist", async ([AsParameters] GetWishlistQuery query, GetWishlistHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(query, cancellationToken)).ToHttpResult())
            .AddEndpointFilter<ValidationFilter<GetWishlistQuery>>()
            .WithTags("Wishlist")
            .ProducesApiResponse<PagedResponse<WishlistItemResponse>>()
            .ProducesApiErrors(StatusCodes.Status400BadRequest);
}
