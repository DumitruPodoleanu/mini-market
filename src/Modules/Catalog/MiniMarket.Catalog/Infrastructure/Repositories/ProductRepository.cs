using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;
using MiniMarket.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class ProductRepository(ICatalogDbContext dbContext)
    : Repository<ProductEntity>(dbContext), IProductRepository
{
    private const string LikeEscapeCharacter = "\\";

    private IQueryable<ProductEntity> ProductsWithDetails =>
        Entities
            .Include(p => p.Slug)
            .Include(p => p.Brand).ThenInclude(b => b!.Slug)
            .Include(p => p.Vendor).ThenInclude(v => v!.Slug)
            .Include(p => p.Categories).ThenInclude(c => c.Slug);

    public Task<ProductEntity?> GetByVendorAndSkuForUpdateAsync(Guid vendorId, string sku, CancellationToken cancellationToken) =>
        ProductsWithDetails
            .FirstOrDefaultAsync(p => p.VendorId == vendorId && p.Sku == sku, cancellationToken);

    public Task<ProductEntity?> GetBySlugForUpdateAsync(string slug, CancellationToken cancellationToken) =>
        ProductsWithDetails
            .FirstOrDefaultAsync(p => p.Slug!.Value == slug, cancellationToken);

    public Task<ProductEntity?> GetBySlugAsync(string slug, CancellationToken cancellationToken) =>
        ProductsWithDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug!.Value == slug, cancellationToken);

    public async Task<IReadOnlyList<ProductEntity>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        await Entities
            .AsNoTracking()
            .Include(p => p.Slug)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);

    public async Task<(IReadOnlyList<ProductEntity> Items, int TotalItems)> GetPageAsync(
        ProductFilter filter, CancellationToken cancellationToken)
    {
        var query = ProductsWithDetails.AsNoTracking();

        if (filter.CategorySlug is not null)
            query = query.Where(p => p.Categories.Any(c => c.Slug!.Value == filter.CategorySlug));

        if (filter.Name is not null)
        {
            var pattern = $"%{EscapeLikePattern(filter.Name)}%";
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, pattern, LikeEscapeCharacter)
                || p.Translations.Any(t => EF.Functions.ILike(t.Name!, pattern, LikeEscapeCharacter)));
        }

        if (filter.Published is not null)
            query = query.Where(p => p.Published == filter.Published);

        if (filter.MinPrice is not null)
            query = query.Where(p => p.Price >= filter.MinPrice);

        if (filter.MaxPrice is not null)
            query = query.Where(p => p.Price <= filter.MaxPrice);

        if (filter.BrandSlug is not null)
            query = query.Where(p => p.Brand!.Slug!.Value == filter.BrandSlug);

        var totalItems = await query.CountAsync(cancellationToken);

        var sorted = filter.Sort switch
        {
            ProductSort.PriceAsc => query.OrderBy(p => p.Price),
            ProductSort.PriceDesc => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        var items = await sorted
            .ThenBy(p => p.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalItems);
    }

    public override async Task DeleteAsync(ProductEntity entity, CancellationToken cancellationToken)
    {
        Entities.Remove(entity);
        DbContext.Set<SlugEntity>().Remove(entity.Slug!);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private static string EscapeLikePattern(string value) =>
        value
            .Replace(LikeEscapeCharacter, LikeEscapeCharacter + LikeEscapeCharacter)
            .Replace("%", LikeEscapeCharacter + "%")
            .Replace("_", LikeEscapeCharacter + "_");
}
