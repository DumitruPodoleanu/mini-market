using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.DeleteProduct;

internal sealed class DeleteProductHandler(
    IProductRepository productRepository,
    ILogger<DeleteProductHandler> logger)
{
    public async Task<Result> HandleAsync(Guid id)
    {
        var product = await productRepository.GetByIdForUpdateAsync(id);

        if (product is null)
            return ProductErrors.NotFound(id);

        await productRepository.DeleteAsync(product);

        logger.LogInformation("Product {ProductId} deleted", product.Id);

        return Result.Success();
    }
}
