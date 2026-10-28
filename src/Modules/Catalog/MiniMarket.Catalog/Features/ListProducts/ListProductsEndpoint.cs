using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.ListProducts;

internal static class ListProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapGet("/", async (string? categorySlug, ListProductsHandler handler) =>
            (await handler.HandleAsync(categorySlug)).ToHttpResult());
}