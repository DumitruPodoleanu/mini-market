namespace MiniMarket.Catalog.Domain;

internal sealed class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Published { get; set; }
    public int StockQuantity { get; set; }
    public Guid BrandId { get; set; }
    public Brand? Brand { get; set; }
    public List<Category> Categories { get; set; } = [];
}