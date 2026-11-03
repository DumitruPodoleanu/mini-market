using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;

namespace MiniMarket.Catalog.Infrastructure.Persistence;

internal sealed class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options), ICatalogDbContext
{
    public const string Schema = "catalog";

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    public DbSet<VendorEntity> Vendors => Set<VendorEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<SlugEntity> Slugs => Set<SlugEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}