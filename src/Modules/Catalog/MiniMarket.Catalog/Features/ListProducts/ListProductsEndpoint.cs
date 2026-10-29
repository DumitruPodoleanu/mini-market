using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.ListProducts;

internal sealed class ListProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products", async (string? categorySlug, ListProductsHandler handler) =>
            (await handler.HandleAsync(categorySlug)).ToHttpResult())
        .WithTags("Products")
        .ProducesApiResponse<IReadOnlyList<ProductResponse>>();
}