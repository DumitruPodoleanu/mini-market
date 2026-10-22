using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Domain;

internal sealed class Product
{
    private const int MaxSkuLength = 50;
    private const int MaxNameLength = 100;

    private readonly List<Category> _categories = [];

    public Guid Id { get; private set; }
    public string Sku { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public bool Published { get; private set; }
    public int StockQuantity { get; private set; }
    public Guid BrandId { get; private set; }
    public Brand? Brand { get; private set; }
    public IReadOnlyCollection<Category> Categories => _categories;

    private Product(Guid id, string sku, string name, decimal price, bool published, int stockQuantity, Guid brandId)
    {
        Id = id;
        SetSku(sku);
        SetName(name);
        SetPrice(price);
        SetStockQuantity(stockQuantity);
        Published = published;
        BrandId = brandId;
    }

    public static Product Create(
        Guid id,
        string sku,
        string name,
        decimal price,
        bool published,
        int stockQuantity,
        Guid brandId,
        IReadOnlyCollection<Category> categories)
    {
        var product = new Product(id, sku, name, price, published, stockQuantity, brandId);
        product.SetCategories(categories);
        return product;
    }

    public void Update(
        string sku,
        string name,
        decimal price,
        bool published,
        int stockQuantity,
        Guid brandId,
        IReadOnlyCollection<Category> categories)
    {
        SetSku(sku);
        SetName(name);
        SetPrice(price);
        SetStockQuantity(stockQuantity);
        Published = published;
        BrandId = brandId;
        SetCategories(categories);
    }

    [MemberNotNull(nameof(Sku))]
    private void SetSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU cannot be empty.");

        var normalized = sku.Trim().ToUpperInvariant();

        if (normalized.Length > MaxSkuLength)
            throw new DomainException($"Product SKU cannot exceed {MaxSkuLength} characters.");
        if (!Regex.IsMatch(normalized, "^[A-Z0-9]+(-[A-Z0-9]+)*$"))
            throw new DomainException("Product SKU can only contain letters, digits and dashes.");

        Sku = normalized;
    }

    [MemberNotNull(nameof(Name))]
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        if (name.Trim().Length > MaxNameLength)
            throw new DomainException($"Product name cannot exceed {MaxNameLength} characters.");

        Name = name.Trim();
    }

    private void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Product price must be greater than zero.");

        Price = price;
    }

    private void SetStockQuantity(int stockQuantity)
    {
        if (stockQuantity < 0)
            throw new DomainException("Product stock quantity cannot be negative.");

        StockQuantity = stockQuantity;
    }

    private void SetCategories(IReadOnlyCollection<Category> categories)
    {
        if (categories.Count == 0)
            throw new DomainException("A product must have at least one category.");

        _categories.Clear();
        _categories.AddRange(categories.DistinctBy(c => c.Id));
    }
}