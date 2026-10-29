using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal sealed class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/api/products/{id:guid}", async (Guid id, UpdateProductRequest request, UpdateProductHandler handler) =>
            (await handler.HandleAsync(id, request)).ToHttpResult())
        .AddEndpointFilter<ValidationFilter<UpdateProductRequest>>()
        .WithTags("Products")
        .ProducesApiResponse<object>()
        .ProducesApiErrors(StatusCodes.Status400BadRequest, StatusCodes.Status404NotFound, StatusCodes.Status409Conflict);
}
