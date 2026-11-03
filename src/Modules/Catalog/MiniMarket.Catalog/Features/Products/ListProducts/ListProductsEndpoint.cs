using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;
using MiniMarket.Catalog.Features.Common.Validation;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.ListProducts;

internal sealed class ListProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/api/products", async ([AsParameters] ListProductsQuery query, ListProductsHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(query, cancellationToken)).ToHttpResult())
            .AddEndpointFilter<ValidationFilter<ListProductsQuery>>()
            .WithTags("Products")
            .ProducesApiResponse<PagedResponse<ProductResponse>>()
            .ProducesApiErrors(StatusCodes.Status400BadRequest);
}