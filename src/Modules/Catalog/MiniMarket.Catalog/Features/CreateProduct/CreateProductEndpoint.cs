using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MiniMarket.Catalog.Features.Common;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapPost("/", async (CreateProductRequest request, CreateProductHandler handler) =>
            (await handler.HandleAsync(request)).ToCreatedHttpResult("/api/products"))
        .AddEndpointFilter<ValidationFilter<CreateProductRequest>>();
}
