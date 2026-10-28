using Microsoft.AspNetCore.Http;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccess
            ? Results.Ok(new ApiResponse<T>(true, result.Value, []))
            : ToFailure(result.Error!);

    public static IResult ToHttpResult(this Result result) =>
        result.IsSuccess
            ? Results.Ok(new ApiResponse<object>(true, null, []))
            : ToFailure(result.Error!);

    private static IResult ToFailure(Error error) =>
        Results.Json(
            new ApiResponse<object>(false, null, [error.Message]),
            statusCode: ToStatusCode(error.Type));

    private static int ToStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}