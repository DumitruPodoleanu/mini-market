using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProductEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductEntity>> GetAllAsync(string? categorySlug, CancellationToken cancellationToken);
    Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId, CancellationToken cancellationToken);
    Task AddAsync(ProductEntity product, CancellationToken cancellationToken);
    Task UpdateAsync(ProductEntity product, CancellationToken cancellationToken);
    Task DeleteAsync(ProductEntity product, CancellationToken cancellationToken);
}