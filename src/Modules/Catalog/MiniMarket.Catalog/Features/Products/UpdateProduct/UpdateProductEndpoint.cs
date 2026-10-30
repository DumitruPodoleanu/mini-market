using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;
using MiniMarket.Catalog.Features.Common.Validation;

namespace MiniMarket.Catalog.Features.Products.UpdateProduct;

internal sealed class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/api/products/{id:guid}", async (Guid id, UpdateProductRequest request, UpdateProductHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(id, request, cancellationToken)).ToHttpResult())
        .AddEndpointFilter<ValidationFilter<UpdateProductRequest>>()
        .WithTags("Products")
        .ProducesApiResponse<object>()
        .ProducesApiErrors(StatusCodes.Status400BadRequest, StatusCodes.Status404NotFound, StatusCodes.Status409Conflict);
}
