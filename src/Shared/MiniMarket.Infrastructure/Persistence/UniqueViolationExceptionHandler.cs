using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Shared.Results;
using Npgsql;

namespace MiniMarket.Infrastructure.Persistence;

public sealed class UniqueViolationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } })
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(
            new ApiResponse<object>(false, null, ["The request conflicts with a change made at the same time. Try again."]),
            cancellationToken);

        return true;
    }
}
