namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class BrandEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Slug { get; set; }
}