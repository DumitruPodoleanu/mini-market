using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class BrandRepository(ICatalogDbContext dbContext) : Repository<BrandEntity>(dbContext), IBrandRepository
{
    public Task<BrandEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken) =>
        Entities.Include(b => b.Slug).FirstOrDefaultAsync(b => b.Slug!.Value == slug, cancellationToken);
}
