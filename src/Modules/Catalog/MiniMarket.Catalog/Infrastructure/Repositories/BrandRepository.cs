using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class BrandRepository(ICatalogDbContext dbContext) : IBrandRepository
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Brands.AnyAsync(b => b.Id == id, cancellationToken);
}
