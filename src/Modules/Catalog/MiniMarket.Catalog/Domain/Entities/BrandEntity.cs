using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class BrandEntity : SluggableEntity
{
    public required string Name { get; set; }
}