using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class ProductRepository(ICatalogDbContext dbContext) : IProductRepository
{
    private IQueryable<ProductEntity> ProductsWithDetails =>
        dbContext.Products
            .Include(p => p.Brand)
            .Include(p => p.Categories);

    public Task<ProductEntity?> GetByIdAsync(Guid id) =>
        ProductsWithDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task<ProductEntity?> GetByIdForUpdateAsync(Guid id) =>
        ProductsWithDetails
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IReadOnlyList<ProductEntity>> GetAllAsync(string? categorySlug)
    {
        var query = ProductsWithDetails.AsNoTracking();

        if (categorySlug is not null)
            query = query.Where(p => p.Categories.Any(c => c.Slug == categorySlug));

        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    public Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId = null) =>
        dbContext.Products.AnyAsync(p => p.Sku == sku && p.Id != excludedProductId);

    public async Task AddAsync(ProductEntity product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductEntity product)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProductEntity product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}