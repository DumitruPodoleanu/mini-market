using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.DeleteProduct;

internal static class DeleteProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapDelete("/{id:guid}", async (Guid id, DeleteProductHandler handler) =>
            (await handler.HandleAsync(id)).ToHttpResult());
}
