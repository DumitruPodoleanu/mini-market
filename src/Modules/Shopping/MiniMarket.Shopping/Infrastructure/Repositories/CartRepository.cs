using Microsoft.EntityFrameworkCore;
using MiniMarket.Infrastructure.Persistence;
using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Domain.Repositories;
using MiniMarket.Shopping.Infrastructure.Persistence;

namespace MiniMarket.Shopping.Infrastructure.Repositories;

internal sealed class CartRepository(IShoppingDbContext dbContext) : Repository<CartItemEntity>(dbContext), ICartRepository
{
    public async Task<IReadOnlyList<CartItemEntity>> GetAllAsync(Guid userId, CancellationToken cancellationToken) =>
        await Entities.AsNoTracking()
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.Id)
            .ToListAsync(cancellationToken);

    public async Task<bool> UpdateQuantityAsync(Guid userId, Guid productId, int quantity, CancellationToken cancellationToken) =>
        await Entities
            .Where(c => c.UserId == userId && c.ProductId == productId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(c => c.Quantity, quantity), cancellationToken) > 0;

    public async Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        await Entities
            .Where(c => c.UserId == userId && c.ProductId == productId)
            .ExecuteDeleteAsync(cancellationToken) > 0;

    public Task DeleteAllAsync(Guid userId, CancellationToken cancellationToken) =>
        Entities.Where(c => c.UserId == userId).ExecuteDeleteAsync(cancellationToken);
}
