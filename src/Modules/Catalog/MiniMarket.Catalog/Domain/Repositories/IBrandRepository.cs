using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IBrandRepository : IRepository<BrandEntity>
{
    Task<BrandEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}
