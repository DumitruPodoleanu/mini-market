using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<T>().First();
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }
        return await next(context);
    }
}