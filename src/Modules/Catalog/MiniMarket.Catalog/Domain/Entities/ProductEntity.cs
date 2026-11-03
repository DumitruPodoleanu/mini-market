using MiniMarket.Catalog.Domain.Entities.Common;
using MiniMarket.Catalog.Domain.Entities.Translations;

namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class ProductEntity : TranslatableEntity<ProductTranslationEntity>
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool Published { get; set; }
    public int StockQuantity { get; set; }
    public Guid BrandId { get; set; }
    public BrandEntity? Brand { get; set; }
    public Guid VendorId { get; set; }
    public VendorEntity? Vendor { get; set; }
    public List<CategoryEntity> Categories { get; set; } = [];
}