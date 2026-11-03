namespace MiniMarket.Catalog.Features.Common.Http;

internal sealed record UpsertResult<T>(T Value, bool Created);