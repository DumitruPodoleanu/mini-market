using Microsoft.EntityFrameworkCore;
using MiniMarket.Infrastructure.Persistence;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Infrastructure.Persistence;

namespace MiniMarket.Shopping.Infrastructure.Repositories;

internal sealed class WishlistRepository(IShoppingDbContext dbContext) : Repository<WishlistItemEntity>(dbContext), IWishlistRepository
{
    public Task<WishlistItemEntity?> GetAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        Entities.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId, cancellationToken);

    public async Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        await Entities
            .Where(w => w.UserId == userId && w.ProductId == productId)
            .ExecuteDeleteAsync(cancellationToken) > 0;

    public async Task<(IReadOnlyList<WishlistItemEntity> Items, int TotalItems)> GetPageAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = Entities.AsNoTracking().Where(w => w.UserId == userId);
        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalItems);
    }
}