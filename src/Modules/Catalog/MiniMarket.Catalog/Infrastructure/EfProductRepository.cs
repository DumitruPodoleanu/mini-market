using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure;

internal sealed class EfProductRepository(ICatalogDbContext dbContext) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id) =>
         dbContext.Products
             .Include(p => p.Brand)
             .Include(p => p.Categories)
             .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IReadOnlyList<Product>> GetAllAsync(string? categorySlug)
    {
        IQueryable<Product> query = dbContext.Products
            .Include(p => p.Brand)
            .Include(p => p.Categories);

        if (categorySlug is not null)
            query = query.Where(p => p.Categories.Any(c => c.Slug == categorySlug));

        return await query.OrderBy(p => p.Name).ToListAsync();
    }

    public Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId = null) =>
        dbContext.Products.AnyAsync(p => p.Sku == sku && p.Id != excludedProductId);

    public async Task AddAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}