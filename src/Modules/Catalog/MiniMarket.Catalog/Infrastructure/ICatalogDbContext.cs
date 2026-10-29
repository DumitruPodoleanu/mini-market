using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal interface ICatalogDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Brand> Brands { get; }
    DbSet<Category> Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}