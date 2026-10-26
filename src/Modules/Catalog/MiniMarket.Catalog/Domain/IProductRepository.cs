namespace MiniMarket.Catalog.Domain;

internal interface IProductRepository
{

    Task<Product?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Product>> GetAllAsync(string? categorySlug);
    Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId = null);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}