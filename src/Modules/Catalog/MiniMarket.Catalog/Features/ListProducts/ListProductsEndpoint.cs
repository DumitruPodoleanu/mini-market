using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.ListProducts;

internal static class ListProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapGet("/", async (string? categorySlug, ListProductsHandler handler) =>
            Results.Ok(await handler.HandleAsync(categorySlug)));
}