using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;
using MiniMarket.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class CategoryRepository(ICatalogDbContext dbContext)
    : Repository<CategoryEntity>(dbContext), ICategoryRepository
{
    public async Task<IReadOnlyList<CategoryEntity>> GetBySlugsAsync(IReadOnlyCollection<string> slugs, CancellationToken cancellationToken) =>
        await Entities.Include(c => c.Slug).Where(c => slugs.Contains(c.Slug!.Value)).ToListAsync(cancellationToken);
}
