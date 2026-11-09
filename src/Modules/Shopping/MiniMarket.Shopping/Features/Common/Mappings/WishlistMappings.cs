using MiniMarket.Catalog.Contracts;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Common.Responses;

namespace MiniMarket.Shopping.Features.Common.Mappings;

internal static class WishlistMappings
{
    public static WishlistItemEntity ToWishlistItemEntity(this ProductSummary product, Guid userId) => new()
    {
        Id = Guid.CreateVersion7(),
        UserId = userId,
        ProductId = product.Id
    };

    public static WishlistItemResponse ToResponse(this WishlistItemEntity item, ProductSummary product) => new(
        product.Slug,
        product.Name,
        product.Price,
        product.Published && product.StockQuantity > 0
    );
}
