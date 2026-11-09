using MiniMarket.Shared.Domain;
using MiniMarket.Shopping.Domain.Entities;

namespace MiniMarket.Shopping.Domain.Repositories;

internal interface ICartRepository : IRepository<CartItemEntity>
{
    Task<IReadOnlyList<CartItemEntity>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> UpdateQuantityAsync(Guid userId, Guid productId, int quantity, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId, Guid productId, CancellationToken cancellationToken);
    Task DeleteAllAsync(Guid userId, CancellationToken cancellationToken);
}
