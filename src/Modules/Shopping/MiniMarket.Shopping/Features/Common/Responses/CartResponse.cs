namespace MiniMarket.Shopping.Features.Common.Responses;

internal sealed record CartResponse(IReadOnlyList<CartItemResponse> Items, decimal Total);
