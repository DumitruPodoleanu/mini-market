namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class CategoryEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
}