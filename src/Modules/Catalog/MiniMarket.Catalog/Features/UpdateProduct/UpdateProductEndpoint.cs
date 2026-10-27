using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder group) =>
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, UpdateProductHandler handler, IValidator<UpdateProductRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return Results.ValidationProblem(validation.ToDictionary());
            }
            await handler.HandleAsync(id, request);
            return Results.NoContent();
        });
}
