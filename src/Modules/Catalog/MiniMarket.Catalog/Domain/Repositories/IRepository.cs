using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}