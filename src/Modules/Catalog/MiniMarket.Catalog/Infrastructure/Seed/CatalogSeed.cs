using MiniMarket.Catalog.Domain;

namespace MiniMarket.Catalog.Infrastructure.Seed;

internal static class CatalogSeed
{
    private static readonly Guid NowFoods = Guid.Parse("b0000000-0000-0000-0000-000000000001");
    private static readonly Guid Solgar = Guid.Parse("b0000000-0000-0000-0000-000000000002");
    private static readonly Guid NaturesWay = Guid.Parse("b0000000-0000-0000-0000-000000000003");

    private static readonly Guid Supplements = Guid.Parse("c0000000-0000-0000-0000-000000000001");
    private static readonly Guid Adaptogens = Guid.Parse("c0000000-0000-0000-0000-000000000002");
    private static readonly Guid Vitamins = Guid.Parse("c0000000-0000-0000-0000-000000000003");
    private static readonly Guid Minerals = Guid.Parse("c0000000-0000-0000-0000-000000000004");

    private static readonly Guid Ashwagandha = Guid.Parse("a0000000-0000-0000-0000-000000000001");
    private static readonly Guid VitaminD3 = Guid.Parse("a0000000-0000-0000-0000-000000000002");
    private static readonly Guid Magnesium = Guid.Parse("a0000000-0000-0000-0000-000000000003");
    private static readonly Guid Rhodiola = Guid.Parse("a0000000-0000-0000-0000-000000000004");

    public static readonly Brand[] Brands =
    [
        new() { Id = NowFoods, Name = "Now Foods", Slug = "now-foods" },
        new() { Id = Solgar, Name = "Solgar", Slug = "solgar" },
        new() { Id = NaturesWay, Name = "Nature's Way", Slug = "natures-way" },
    ];

    public static readonly Category[] Categories =
    [
        new() { Id = Supplements, Name = "Suplimente", Slug = "suplimente" },
        new() { Id = Adaptogens, Name = "Plante adaptogene", Slug = "plante-adaptogene" },
        new() { Id = Vitamins, Name = "Vitamine", Slug = "vitamine" },
        new() { Id = Minerals, Name = "Minerale", Slug = "minerale" },
    ];

    public static readonly object[] Products =
    [
        new { Id = Ashwagandha, Sku = "HERB-001", Name = "Ashwagandha 60 caps", Price = 189.90m, Published = true, StockQuantity = 25, BrandId = NowFoods },
        new { Id = VitaminD3, Sku = "VIT-001", Name = "Vitamina D3 2000 UI 120 caps", Price = 149.50m, Published = true, StockQuantity = 40, BrandId = Solgar },
        new { Id = Magnesium, Sku = "MIN-001", Name = "Magneziu bisglicinat 90 tab", Price = 219.00m, Published = true, StockQuantity = 12, BrandId = NaturesWay },
        new { Id = Rhodiola, Sku = "HERB-002", Name = "Rhodiola 60 caps", Price = 249.00m, Published = false, StockQuantity = 0, BrandId = NowFoods },
    ];

    public static readonly object[] ProductCategories =
    [
        new { ProductId = Ashwagandha, CategoryId = Supplements },
        new { ProductId = Ashwagandha, CategoryId = Adaptogens },
        new { ProductId = VitaminD3, CategoryId = Supplements },
        new { ProductId = VitaminD3, CategoryId = Vitamins },
        new { ProductId = Magnesium, CategoryId = Supplements },
        new { ProductId = Magnesium, CategoryId = Minerals },
        new { ProductId = Rhodiola, CategoryId = Adaptogens },
    ];
}