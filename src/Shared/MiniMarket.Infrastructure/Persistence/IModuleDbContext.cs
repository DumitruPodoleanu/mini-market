using Microsoft.EntityFrameworkCore;

namespace MiniMarket.Infrastructure.Persistence;

public interface IModuleDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}