using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common.Endpoints;
using MiniMarket.Catalog.Features.Common.Http;

namespace MiniMarket.Catalog.Features.Products.DeleteProduct;

internal sealed class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/api/products/{id:guid}", async (Guid id, DeleteProductHandler handler, CancellationToken cancellationToken) =>
            (await handler.HandleAsync(id, cancellationToken)).ToHttpResult())
        .WithTags("Products")
        .ProducesApiResponse<object>()
        .ProducesApiErrors(StatusCodes.Status404NotFound);
}
