namespace MiniMarket.Shopping.Features.Common.Responses;

internal sealed record CartItemResponse(
    string ProductSlug,
    string Name,
    decimal Price,
    int Quantity,
    decimal LineTotal,
    bool Available);
