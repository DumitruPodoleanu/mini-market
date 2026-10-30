using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;

namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed class ListProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products", async (string? categorySlug, ListProductsHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(categorySlug, cancellationToken)).ToHttpResult())
        .WithTags("Products")
        .ProducesApiResponse<IReadOnlyList<ProductResponse>>();
}