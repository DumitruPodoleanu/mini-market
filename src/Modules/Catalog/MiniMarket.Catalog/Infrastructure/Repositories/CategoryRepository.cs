using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class CategoryRepository(ICatalogDbContext dbContext)
    : Repository<CategoryEntity>(dbContext), ICategoryRepository
{
    public async Task<IReadOnlyList<CategoryEntity>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        await Entities.Where(c => ids.Contains(c.Id)).ToListAsync(cancellationToken);
}
