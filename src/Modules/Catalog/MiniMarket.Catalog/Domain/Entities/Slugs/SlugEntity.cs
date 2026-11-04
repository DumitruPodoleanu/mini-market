using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Entities.Slugs;

internal sealed class SlugEntity : BaseEntity
{
    public required string Value { get; set; }
    public SlugEntityType EntityType { get; set; }
}