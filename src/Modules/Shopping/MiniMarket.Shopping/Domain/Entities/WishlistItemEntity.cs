using MiniMarket.Shared.Domain;

namespace MiniMarket.Shopping.Domain.Entities;

internal sealed class WishlistItemEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}