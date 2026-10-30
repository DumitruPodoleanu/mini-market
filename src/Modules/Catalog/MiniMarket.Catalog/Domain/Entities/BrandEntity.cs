namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class BrandEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
}