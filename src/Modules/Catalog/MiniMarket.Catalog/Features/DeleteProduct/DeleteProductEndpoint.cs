using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.DeleteProduct;

internal static class DeleteProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapDelete("/{id:guid}", async (Guid id, DeleteProductHandler handler) =>
        {
            await handler.HandleAsync(id);
            return Results.NoContent();
        });
}
