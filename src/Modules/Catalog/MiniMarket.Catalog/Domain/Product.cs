using MiniMarket.Shared;

namespace MiniMarket.Catalog.Domain;

internal sealed class Product
{
    private const int MaxNameLength = 100;
    private const int MaxDiscountPercentage = 90;

    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int DiscountPercentage { get; private set; }

    public Product(Guid id, string name, decimal price, int discountPercentage)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");
        if (name.Trim().Length > MaxNameLength)
            throw new DomainException($"Product name cannot exceed {MaxNameLength} characters.");
        if (price <= 0)
            throw new DomainException("Product price must be greater than zero.");

        Id = id;
        Name = name.Trim();
        Price = price;
        ApplyDiscount(discountPercentage);

    }

    public void ApplyDiscount(int percent)
    {
        if (percent is < 0 or > MaxDiscountPercentage)
            throw new DomainException($"Product discount percentage must be between 0 and {MaxDiscountPercentage}.");

        DiscountPercentage = percent;
    }

    public decimal FinalPrice =>
    Math.Round(Price * (1 - DiscountPercentage / 100m), 2, MidpointRounding.AwayFromZero);
}