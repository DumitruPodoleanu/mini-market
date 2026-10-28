namespace MiniMarket.Shared.Results;

public sealed record Error(string Message, ErrorType Type);
