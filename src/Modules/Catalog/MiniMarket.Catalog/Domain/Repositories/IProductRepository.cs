using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IProductRepository : IRepository<ProductEntity>
{
    Task<ProductEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductEntity>> GetAllAsync(string? categorySlug, CancellationToken cancellationToken);
    Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId, CancellationToken cancellationToken);
}