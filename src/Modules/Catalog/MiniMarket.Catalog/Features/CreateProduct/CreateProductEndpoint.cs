using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapPost("/", async (CreateProductRequest request, CreateProductHandler handler) =>
        {
            var id = await handler.HandleAsync(request);
            return Results.Created($"/api/products/{id}", new { id });
        });
}
