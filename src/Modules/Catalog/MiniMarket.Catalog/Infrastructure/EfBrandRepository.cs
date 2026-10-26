using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class EfBrandRepository(CatalogDbContext dbContext) : IBrandRepository
{
    public Task<bool> ExistsAsync(Guid id) =>
        dbContext.Brands.AnyAsync(b => b.Id == id);
}
