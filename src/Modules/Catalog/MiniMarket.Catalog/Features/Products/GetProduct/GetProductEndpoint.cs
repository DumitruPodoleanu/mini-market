using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Infrastructure.Endpoints;
using MiniMarket.Infrastructure.Http;

namespace MiniMarket.Catalog.Features.Products.GetProduct;

internal sealed class GetProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products/{slug}", async (string slug, GetProductHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(slug, cancellationToken)).ToHttpResult())
            .WithTags("Products")
            .ProducesApiResponse<ProductResponse>()
            .ProducesApiErrors(StatusCodes.Status404NotFound);
}
