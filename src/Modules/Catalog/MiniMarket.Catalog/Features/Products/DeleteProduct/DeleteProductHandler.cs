using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.DeleteProduct;

internal sealed class DeleteProductHandler(
    IProductRepository productRepository,
    ILogger<DeleteProductHandler> logger)
{
    public async Task<Result> HandleAsync(string slug, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetBySlugForUpdateAsync(slug, cancellationToken);

        if (product is null)
            return ProductErrors.NotFound(slug);

        await productRepository.DeleteAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} deleted", product.Id);

        return Result.Success();
    }
}
