namespace MiniMarket.Shopping.Features.Common.Responses;

internal sealed record WishlistItemResponse(string ProductSlug, string Name, decimal Price, bool Available);
