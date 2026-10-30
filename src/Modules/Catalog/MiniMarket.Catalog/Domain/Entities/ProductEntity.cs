namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class ProductEntity
{
    public Guid Id { get; set; }
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public bool Published { get; set; }
    public int StockQuantity { get; set; }
    public Guid BrandId { get; set; }
    public BrandEntity? Brand { get; set; }
    public List<CategoryEntity> Categories { get; set; } = [];
}