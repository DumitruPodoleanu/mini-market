using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Contracts;
using MiniMarket.Shared.Results;
using MiniMarket.Shopping.Features.Common.Errors;

namespace MiniMarket.Shopping.Features.Common;

internal sealed class ProductLookup(ICatalogApi catalogApi, ILogger<ProductLookup> logger)
{
    public async Task<Result<ProductSummary>> GetAsync(string productSlug, CancellationToken cancellationToken)
    {
        var product = await catalogApi.GetProductBySlugAsync(productSlug, cancellationToken);
        if (product is not null)
            return product;

        var error = ShoppingErrors.ProductNotFound(productSlug);
        logger.LogWarning("Product lookup failed: {Error}", error.Message);
        return error;
    }

    public async Task<Result<ProductSummary>> GetPublishedAsync(string productSlug, CancellationToken cancellationToken)
    {
        var result = await GetAsync(productSlug, cancellationToken);
        if (!result.IsSuccess || result.Value!.Published)
            return result;

        var error = ShoppingErrors.ProductNotPublished(productSlug);
        logger.LogWarning("Product lookup failed: {Error}", error.Message);
        return error;
    }
}
