using MiniMarket.Catalog.Contracts;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Features.Common.Responses;

namespace MiniMarket.Shopping.Features.Common.Mappings;

internal static class CartMappings
{
    public static CartItemEntity ToCartItemEntity(this ProductSummary product, Guid userId, int quantity) => new()
    {
        Id = Guid.CreateVersion7(),
        UserId = userId,
        ProductId = product.Id,
        Quantity = quantity
    };

    public static CartItemResponse ToResponse(this CartItemEntity item, ProductSummary product) => new(
        product.Slug,
        product.Name,
        product.Price,
        item.Quantity,
        product.Price * item.Quantity,
        product.Published && product.StockQuantity >= item.Quantity
    );
}
