using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IBrandRepository : IRepository<BrandEntity>
{
    Task<BrandEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}
