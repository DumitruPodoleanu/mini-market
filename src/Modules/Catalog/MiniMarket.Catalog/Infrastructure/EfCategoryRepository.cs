using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class EfCategoryRepository(CatalogDbContext dbContext) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetByIdsAsync(IReadOnlyCollection<Guid> ids) =>
        await dbContext.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
}
