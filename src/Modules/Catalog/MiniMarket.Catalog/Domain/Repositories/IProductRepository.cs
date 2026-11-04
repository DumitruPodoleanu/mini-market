using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IProductRepository : IRepository<ProductEntity>
{
    Task<(IReadOnlyList<ProductEntity> Items, int TotalItems)> GetPageAsync(ProductFilter filter, CancellationToken cancellationToken);
    Task<ProductEntity?> GetByVendorAndSkuForUpdateAsync(Guid vendorId, string sku, CancellationToken cancellationToken);
    Task<ProductEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<ProductEntity?> GetBySlugForUpdateAsync(string slug, CancellationToken cancellationToken);
}