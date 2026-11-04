namespace MiniMarket.Catalog.Contracts;

public sealed record ProductSummary(
    Guid Id,
    string Slug,
    string Name,
    decimal Price,
    int StockQuantity,
    bool Published);