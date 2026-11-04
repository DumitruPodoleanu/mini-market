namespace MiniMarket.Infrastructure.Http;

public sealed record UpsertResult<T>(T Value, bool Created);