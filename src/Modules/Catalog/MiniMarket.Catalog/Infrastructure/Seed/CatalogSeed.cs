using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;

namespace MiniMarket.Catalog.Infrastructure.Seed;

internal static class CatalogSeed
{
    public static void Seed(DbContext context)
    {
        if (context.Set<BrandEntity>().Any())
        {
            return;
        }

        context.AddRange(CreateProducts());
        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        if (await context.Set<BrandEntity>().AnyAsync(cancellationToken))
        {
            return;
        }

        context.AddRange(CreateProducts());
        await context.SaveChangesAsync(cancellationToken);
    }

    private static ProductEntity[] CreateProducts()
    {
        var nowFoods = new BrandEntity { Id = Guid.CreateVersion7(), Name = "Now Foods", Slug = "now-foods" };
        var solgar = new BrandEntity { Id = Guid.CreateVersion7(), Name = "Solgar", Slug = "solgar" };
        var naturesWay = new BrandEntity { Id = Guid.CreateVersion7(), Name = "Nature's Way", Slug = "natures-way" };

        var supplements = new CategoryEntity { Id = Guid.CreateVersion7(), Name = "Suplimente", Slug = "suplimente" };
        var adaptogens = new CategoryEntity { Id = Guid.CreateVersion7(), Name = "Plante adaptogene", Slug = "plante-adaptogene" };
        var vitamins = new CategoryEntity { Id = Guid.CreateVersion7(), Name = "Vitamine", Slug = "vitamine" };
        var minerals = new CategoryEntity { Id = Guid.CreateVersion7(), Name = "Minerale", Slug = "minerale" };

        return
        [
            new() { Id = Guid.CreateVersion7(), Sku = "HERB-001", Name = "Ashwagandha 60 caps", Price = 189.90m, Published = true, StockQuantity = 25, Brand = nowFoods, Categories = [supplements, adaptogens] },
            new() { Id = Guid.CreateVersion7(), Sku = "VIT-001", Name = "Vitamina D3 2000 UI 120 caps", Price = 149.50m, Published = true, StockQuantity = 40, Brand = solgar, Categories = [supplements, vitamins] },
            new() { Id = Guid.CreateVersion7(), Sku = "MIN-001", Name = "Magneziu bisglicinat 90 tab", Price = 219.00m, Published = true, StockQuantity = 12, Brand = naturesWay, Categories = [supplements, minerals] },
            new() { Id = Guid.CreateVersion7(), Sku = "HERB-002", Name = "Rhodiola 60 caps", Price = 249.00m, Published = false, StockQuantity = 0, Brand = nowFoods, Categories = [adaptogens] },
        ];
    }
}
