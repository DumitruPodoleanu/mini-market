using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Persistence;

internal interface ICatalogDbContext
{
    DbSet<ProductEntity> Products { get; }
    DbSet<BrandEntity> Brands { get; }
    DbSet<CategoryEntity> Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}