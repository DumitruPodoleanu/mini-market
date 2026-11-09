using Microsoft.Extensions.Logging.Abstractions;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shopping.Features.Common;

namespace MiniMarket.Shopping.Tests.Fakes;

internal static class TestProducts
{
    public static ProductSummary Published(string slug = "published", decimal price = 10m, int stock = 5) =>
        new(Guid.NewGuid(), slug, slug, price, stock, true);

    public static ProductSummary Unpublished(string slug = "unpublished") =>
        new(Guid.NewGuid(), slug, slug, 10m, 5, false);

    public static ProductLookup Lookup(ICatalogApi catalogApi) =>
        new(catalogApi, NullLogger<ProductLookup>.Instance);
}
