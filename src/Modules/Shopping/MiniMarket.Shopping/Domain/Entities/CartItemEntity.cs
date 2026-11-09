using MiniMarket.Shared.Domain;

namespace MiniMarket.Shopping.Domain.Entities;

internal sealed class CartItemEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
