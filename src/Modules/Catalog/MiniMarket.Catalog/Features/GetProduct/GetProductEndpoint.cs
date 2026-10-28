using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.GetProduct;

internal static class GetProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapGet("/{id:guid}", async (Guid id, GetProductHandler handler) =>
            (await handler.HandleAsync(id)).ToHttpResult());
}
