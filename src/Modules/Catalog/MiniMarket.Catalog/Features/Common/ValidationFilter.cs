using FluentValidation;
using Microsoft.AspNetCore.Http;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().First();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return Results.BadRequest(new ApiResponse<object>(false, null, errors));
        }
        return await next(context);
    }
}