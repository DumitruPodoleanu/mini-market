using Microsoft.EntityFrameworkCore;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Infrastructure.Persistence;

public abstract class Repository<TEntity>(IModuleDbContext dbContext) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected IModuleDbContext DbContext { get; } = dbContext;

    protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

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