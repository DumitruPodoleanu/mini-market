using MiniMarket.Shopping.Domain.Entities;
using MiniMarket.Shopping.Domain.Repositories;

namespace MiniMarket.Shopping.Tests.Fakes;

internal sealed class FakeCartRepository : ICartRepository
{
    public List<CartItemEntity> Items { get; } = [];

    public Task AddAsync(CartItemEntity entity, CancellationToken cancellationToken)
    {
        Items.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(CartItemEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task DeleteAsync(CartItemEntity entity, CancellationToken cancellationToken)
    {
        Items.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateQuantityAsync(Guid userId, Guid productId, int quantity, CancellationToken cancellationToken)
    {
        var item = Items.FirstOrDefault(item => item.UserId == userId && item.ProductId == productId);
        if (item is null)
            return Task.FromResult(false);

        item.Quantity = quantity;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken) =>
        Task.FromResult(Items.RemoveAll(item => item.UserId == userId && item.ProductId == productId) > 0);

    public Task<IReadOnlyList<CartItemEntity>> GetAllAsync(Guid userId, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<CartItemEntity>>(Items.Where(item => item.UserId == userId).ToList());

    public Task DeleteAllAsync(Guid userId, CancellationToken cancellationToken)
    {
        Items.RemoveAll(item => item.UserId == userId);
        return Task.CompletedTask;
    }
}
