using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, UpdateProductHandler handler) =>
        {
            await handler.HandleAsync(id, request);
            return Results.NoContent();
        });
}
