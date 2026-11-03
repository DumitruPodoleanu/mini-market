using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<IReadOnlyList<CategoryEntity>> GetBySlugsAsync(IReadOnlyCollection<string> slugs, CancellationToken cancellationToken);
}
