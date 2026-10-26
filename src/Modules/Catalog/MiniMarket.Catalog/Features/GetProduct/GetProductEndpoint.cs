using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.GetProduct;

internal static class GetProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapGet("/{id:guid}", async (Guid id, GetProductHandler handler) =>
            Results.Ok(await handler.HandleAsync(id)));
}
