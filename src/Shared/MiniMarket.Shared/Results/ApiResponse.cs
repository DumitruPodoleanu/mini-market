namespace MiniMarket.Shared.Results;

public sealed record ApiResponse<T>(bool Success, T? Data, Error? Error);