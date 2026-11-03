using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class VendorRepository(ICatalogDbContext dbContext) : Repository<VendorEntity>(dbContext), IVendorRepository
{
    public Task<VendorEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken) =>
        Entities.Include(v => v.Slug).FirstOrDefaultAsync(v => v.Slug!.Value == slug, cancellationToken);
}