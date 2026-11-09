using MiniMarket.Shared.Domain;
using MiniMarket.Shopping.Domain.Entities;

namespace MiniMarket.Shopping.Domain.Repositories;

internal interface IWishlistRepository : IRepository<WishlistItemEntity>
{
    Task<WishlistItemEntity?> GetAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
    Task<(IReadOnlyList<WishlistItemEntity> Items, int TotalItems)> GetPageAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);
}