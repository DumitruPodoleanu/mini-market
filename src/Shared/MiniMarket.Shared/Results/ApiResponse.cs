namespace MiniMarket.Shared.Results;

public sealed record ApiResponse<T>(bool Success, T? Data, IReadOnlyList<string> Errors);