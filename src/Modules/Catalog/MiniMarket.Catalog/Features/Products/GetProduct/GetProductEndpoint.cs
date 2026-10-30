using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;

namespace MiniMarket.Catalog.Features.Products.GetProduct;

internal sealed class GetProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products/{id:guid}", async (Guid id, GetProductHandler handler) =>
            (await handler.HandleAsync(id)).ToHttpResult())
        .WithTags("Products")
        .ProducesApiResponse<ProductResponse>()
        .ProducesApiErrors(StatusCodes.Status404NotFound);
}
