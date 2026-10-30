using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IProductRepository
{
    Task<ProductEntity?> GetByIdAsync(Guid id);
    Task<ProductEntity?> GetByIdForUpdateAsync(Guid id);
    Task<IReadOnlyList<ProductEntity>> GetAllAsync(string? categorySlug);
    Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId = null);
    Task AddAsync(ProductEntity product);
    Task UpdateAsync(ProductEntity product);
    Task DeleteAsync(ProductEntity product);
}