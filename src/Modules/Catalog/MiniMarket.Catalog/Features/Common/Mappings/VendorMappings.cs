using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Features.Common.Contracts;

namespace MiniMarket.Catalog.Features.Common.Mappings;

internal static class VendorMappings
{
    public static VendorResponse ToResponse(this VendorEntity vendor) => new(
        vendor.Name,
        vendor.Slug!.Value
    );
}