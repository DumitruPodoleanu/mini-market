using Microsoft.AspNetCore.Http;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common.Http;

internal static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result) =>
        result.IsSuccess
            ? Results.Ok(new ApiResponse<T>(true, result.Value, []))
            : ToFailure(result.Error!);

    public static IResult ToNoContentHttpResult(this Result result) =>
        result.IsSuccess ? Results.NoContent() : ToFailure(result.Error!);

    public static IResult ToUpsertHttpResult<T>(this Result<UpsertResult<T>> result, Func<T, string> location)
    {
        if (!result.IsSuccess)
            return ToFailure(result.Error!);

        var value = result.Value!.Value;
        var response = new ApiResponse<T>(true, value, []);

        return result.Value.Created
            ? Results.Created(location(value), response)
            : Results.Ok(response);
    }

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
