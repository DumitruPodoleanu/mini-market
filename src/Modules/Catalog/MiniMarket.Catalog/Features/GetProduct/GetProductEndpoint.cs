using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.GetProduct;

internal sealed class GetProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products/{id:guid}", async (Guid id, GetProductHandler handler) =>
            (await handler.HandleAsync(id)).ToHttpResult())
        .WithTags("Products");
}
