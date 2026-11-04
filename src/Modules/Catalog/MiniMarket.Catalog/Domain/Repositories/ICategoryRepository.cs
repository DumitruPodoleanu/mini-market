using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task<IReadOnlyList<CategoryEntity>> GetBySlugsAsync(IReadOnlyCollection<string> slugs, CancellationToken cancellationToken);
}
