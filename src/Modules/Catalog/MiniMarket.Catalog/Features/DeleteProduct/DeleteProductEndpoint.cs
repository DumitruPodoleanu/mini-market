using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.DeleteProduct;

internal sealed class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/products/{id:guid}", async (Guid id, DeleteProductHandler handler) =>
            (await handler.HandleAsync(id)).ToHttpResult())
        .WithTags("Products")
        .ProducesApiResponse<object>()
        .ProducesApiErrors(StatusCodes.Status404NotFound);
}
