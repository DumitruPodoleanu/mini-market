namespace MiniMarket.Catalog.Features.Common.Contracts;

internal interface IProductRequest
{
    string Sku { get; }
    string Name { get; }
    decimal Price { get; }
    bool Published { get; }
    int StockQuantity { get; }
    Guid BrandId { get; }
    IReadOnlyList<Guid>? CategoryIds { get; }
}