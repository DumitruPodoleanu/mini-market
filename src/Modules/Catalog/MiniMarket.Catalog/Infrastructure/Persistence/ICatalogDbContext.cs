using Microsoft.EntityFrameworkCore;

namespace MiniMarket.Catalog.Infrastructure.Persistence;

internal interface ICatalogDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}