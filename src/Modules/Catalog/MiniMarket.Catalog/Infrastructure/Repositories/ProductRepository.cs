using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class ProductRepository(ICatalogDbContext dbContext)
    : Repository<ProductEntity>(dbContext), IProductRepository
{
    private IQueryable<ProductEntity> ProductsWithDetails =>
        Entities
            .Include(p => p.Brand)
            .Include(p => p.Categories);

    public override Task<ProductEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        ProductsWithDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<ProductEntity?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken) =>
        ProductsWithDetails
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<ProductEntity>> GetAllAsync(string? categorySlug, CancellationToken cancellationToken)
    {
        var query = ProductsWithDetails.AsNoTracking();

        if (categorySlug is not null)
            query = query.Where(p => p.Categories.Any(c => c.Slug == categorySlug));

        return await query.OrderBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public Task<bool> SkuExistsAsync(string sku, Guid? excludedProductId, CancellationToken cancellationToken) =>
        Entities.AnyAsync(p => p.Sku == sku && p.Id != excludedProductId, cancellationToken);
}
