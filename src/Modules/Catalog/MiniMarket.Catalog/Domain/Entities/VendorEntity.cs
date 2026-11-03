using MiniMarket.Catalog.Domain.Entities.Common;

namespace MiniMarket.Catalog.Domain.Entities;

internal sealed class VendorEntity : SluggableEntity
{
    public required string Name { get; set; }
}