using Microsoft.AspNetCore.Mvc;

namespace MiniMarket.Catalog.Features.Products.UpsertProduct;

internal sealed record UpsertProductCommand(
    string? VendorSlug,
    string? Sku,
    [FromBody] UpsertProductRequest Body);