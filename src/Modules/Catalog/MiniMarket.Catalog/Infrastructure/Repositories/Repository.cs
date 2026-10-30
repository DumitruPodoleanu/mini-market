using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal abstract class Repository<TEntity>(ICatalogDbContext dbContext) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected ICatalogDbContext DbContext { get; } = dbContext;

    protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

    public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        Entities.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public virtual Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        Entities.AnyAsync(e => e.Id == id, cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Entities.Add(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Entities.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}