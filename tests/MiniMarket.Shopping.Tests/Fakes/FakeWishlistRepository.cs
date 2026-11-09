using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Domain.Repositories;

namespace MiniMarket.Shopping.Tests.Fakes;

internal sealed class FakeWishlistRepository : IWishlistRepository
{
    public List<WishlistItemEntity> Items { get; } = [];

    public Task AddAsync(WishlistItemEntity entity, CancellationToken cancellationToken)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(WishlistItemEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task DeleteAsync(WishlistItemEntity entity, CancellationToken cancellationToken)
    {
        Items.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<WishlistItemEntity?> GetAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        Task.FromResult(Items.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId));

    public Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        Task.FromResult(Items.RemoveAll(item => item.UserId == userId && item.ProductId == productId) > 0);

    public Task<(IReadOnlyList<WishlistItemEntity> Items, int TotalItems)> GetPageAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var userItems = Items.Where(item => item.UserId == userId).ToList();
        IReadOnlyList<WishlistItemEntity> pageItems = userItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return Task.FromResult((pageItems, userItems.Count));
    }
}
