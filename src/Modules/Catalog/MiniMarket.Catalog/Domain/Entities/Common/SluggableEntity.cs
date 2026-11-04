using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Entities.Common;

internal abstract class SluggableEntity : BaseEntity
{
    public Guid SlugId { get; set; }
    public SlugEntity? Slug { get; set; }
}