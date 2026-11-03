using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;
using MiniMarket.Catalog.Features.Common.Validation;

namespace MiniMarket.Catalog.Features.Products.UpsertProduct;

internal sealed class UpsertProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/api/products", async ([AsParameters] UpsertProductCommand command, UpsertProductHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(command, cancellationToken)).ToUpsertHttpResult(product => $"/api/products/{product.Slug}"))
            .AddEndpointFilter<ValidationFilter<UpsertProductCommand>>()
            .WithTags("Products")
            .ProducesApiResponse<ProductResponse>()
            .ProducesApiResponse<ProductResponse>(StatusCodes.Status201Created)
            .ProducesApiErrors(StatusCodes.Status400BadRequest, StatusCodes.Status404NotFound, StatusCodes.Status409Conflict);
}