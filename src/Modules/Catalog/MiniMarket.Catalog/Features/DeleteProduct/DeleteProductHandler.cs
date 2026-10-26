using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Features.DeleteProduct;

internal sealed class DeleteProductHandler(
    IProductRepository productRepository,
    ILogger<DeleteProductHandler> logger)
{
    public async Task HandleAsync(Guid id)
    {
        var product = await productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product {id} was not found.");

        await productRepository.DeleteAsync(product);

        logger.LogInformation("Product {ProductId} deleted", product.Id);
    }
}
