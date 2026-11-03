using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;

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

    private static SlugEntity CreateSlug(string value, SlugEntityType entityType) =>
        new() { Id = Guid.CreateVersion7(), Value = value, EntityType = entityType };

    private static BrandEntity CreateBrand(string name, string slug) =>
        new() { Id = Guid.CreateVersion7(), Name = name, Slug = CreateSlug(slug, SlugEntityType.Brand) };

    private static VendorEntity CreateVendor(string name, string slug) =>
        new() { Id = Guid.CreateVersion7(), Name = name, Slug = CreateSlug(slug, SlugEntityType.Vendor) };

    private static CategoryEntity CreateCategory(string name, string slug, string nameRu) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Slug = CreateSlug(slug, SlugEntityType.Category),
            Translations = [new() { Language = LanguageConstraints.Russian, Name = nameRu }]
        };

    private static ProductEntity CreateProduct(
        string sku,
        string name,
        string slug,
        decimal price,
        bool published,
        int stockQuantity,
        BrandEntity brand,
        VendorEntity vendor,
        params CategoryEntity[] categories) =>
        new()
        {
            Id = Guid.CreateVersion7(),
            Sku = sku,
            Name = name,
            Slug = CreateSlug(slug, SlugEntityType.Product),
            Price = price,
            Published = published,
            StockQuantity = stockQuantity,
            Brand = brand,
            Vendor = vendor,
            Categories = [.. categories]
        };

    private static ProductEntity WithRussianName(this ProductEntity product, string name)
    {
        product.Translations.Add(new() { Language = LanguageConstraints.Russian, Name = name });
        return product;
    }

    private static ProductEntity[] CreateProducts()
    {
        var samsungStore = CreateVendor("Samsung", "samsung");
        var smartStore = CreateVendor("Smart Store", "smart-store");
        var iStore = CreateVendor("iStore", "istore");

        var apple = CreateBrand("Apple", "apple");
        var samsung = CreateBrand("Samsung", "samsung-2");
        var sony = CreateBrand("Sony", "sony");
        var xiaomi = CreateBrand("Xiaomi", "xiaomi");

        var phones = CreateCategory("Telefoane", "telefoane", "Телефоны");
        var laptops = CreateCategory("Laptopuri", "laptopuri", "Ноутбуки");
        var headphones = CreateCategory("Căști", "casti", "Наушники");
        var tablets = CreateCategory("Tablete", "tablete", "Планшеты");
        var smartwatches = CreateCategory("Smartwatch-uri", "smartwatch-uri", "Смарт-часы");

        return
        [
            CreateProduct("MYNL3", "iPhone 16 Pro 256GB", "iphone-16-pro-256gb", 24999m, true, 15, apple, iStore, phones),
            CreateProduct("SS-IP15-128", "iPhone 15 128GB", "iphone-15-128gb", 14999m, true, 30, apple, smartStore, phones),
            CreateProduct("SM-S938B", "Samsung Galaxy S25 Ultra 256GB", "samsung-galaxy-s25-ultra-256gb", 25999m, true, 10, samsung, samsungStore, phones),
            CreateProduct("SS-GA56-128", "Samsung Galaxy A56 128GB", "samsung-galaxy-a56-128gb", 7499m, true, 45, samsung, smartStore, phones),
            CreateProduct("RN14P-256", "Xiaomi Redmi Note 14 Pro", "xiaomi-redmi-note-14-pro", 6499m, true, 0, xiaomi, smartStore, phones),
            CreateProduct("MTJV3", "AirPods Pro 2", "airpods-pro-2", 4799m, true, 25, apple, iStore, headphones),
            CreateProduct("MTJV3", "AirPods Pro 2", "airpods-pro-2-2", 4599m, true, 8, apple, smartStore, headphones),
            CreateProduct("SM-R630", "Samsung Galaxy Buds3 Pro", "samsung-galaxy-buds3-pro", 3999m, true, 20, samsung, samsungStore, headphones),
            CreateProduct("WH1000XM5B", "Căști Sony WH-1000XM5", "casti-sony-wh-1000xm5", 6299m, true, 12, sony, smartStore, headphones)
                .WithRussianName("Наушники Sony WH-1000XM5"),
            CreateProduct("MW0W3", "Laptop MacBook Air 13 M4 16GB 256GB", "laptop-macbook-air-13-m4-16gb-256gb", 21999m, true, 7, apple, iStore, laptops)
                .WithRussianName("Ноутбук MacBook Air 13 M4 16GB 256GB"),
            CreateProduct("MX2H3", "MacBook Pro 14 M4 Pro", "macbook-pro-14-m4-pro", 41999m, true, 3, apple, iStore, laptops),
            CreateProduct("MC9X4", "iPad Air 11 M3", "ipad-air-11-m3", 12999m, true, 18, apple, iStore, tablets),
            CreateProduct("MWWE3", "Ceas Apple Watch Series 10", "ceas-apple-watch-series-10", 8999m, false, 0, apple, iStore, smartwatches)
                .WithRussianName("Часы Apple Watch Series 10"),
        ];
    }
}
