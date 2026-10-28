using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, UpdateProductHandler handler) =>
        {
            await handler.HandleAsync(id, request);
            return Results.NoContent();
        })
        .AddEndpointFilter<ValidationFilter<UpdateProductRequest>>();
}
