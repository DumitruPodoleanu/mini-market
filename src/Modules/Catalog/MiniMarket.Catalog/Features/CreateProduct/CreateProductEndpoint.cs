using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal sealed class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/api/products", async (CreateProductRequest request, CreateProductHandler handler) =>
            (await handler.HandleAsync(request)).ToCreatedHttpResult("/api/products"))
        .AddEndpointFilter<ValidationFilter<CreateProductRequest>>()
        .WithTags("Products")
        .ProducesApiResponse<Guid>(StatusCodes.Status201Created)
        .ProducesApiErrors(StatusCodes.Status400BadRequest, StatusCodes.Status409Conflict);
}
