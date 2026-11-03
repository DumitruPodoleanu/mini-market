using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IVendorRepository : IRepository<VendorEntity>
{
    Task<VendorEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
}