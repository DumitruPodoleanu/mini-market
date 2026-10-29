using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal static class ApiResponseMetadata
{
    public static RouteHandlerBuilder ProducesApiResponse<T>(this RouteHandlerBuilder builder, int statusCode = StatusCodes.Status200OK) =>
        builder.Produces<ApiResponse<T>>(statusCode);

    public static RouteHandlerBuilder ProducesApiErrors(this RouteHandlerBuilder builder, params int[] statusCodes)
    {
        foreach (var statusCode in statusCodes)
        {
            builder.Produces<ApiResponse<object>>(statusCode);
        }

        return builder;
    }
}
