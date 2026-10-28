using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal sealed class UpdateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<UpdateProductHandler> logger)
{
    public async Task HandleAsync(Guid id, UpdateProductRequest request)
    {
        var product = await productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Product {id} was not found.");

        await productReferences.EnsureBrandExistsAsync(request.BrandId);
        var categories = await productReferences.GetCategoriesAsync(request.CategoryIds ?? []);

        request.MapTo(product, categories);

        if (await productRepository.SkuExistsAsync(product.Sku, product.Id))
            throw new DomainException($"A product with SKU {product.Sku} already exists.");

        await productRepository.UpdateAsync(product);

        logger.LogInformation("Product {ProductId} updated", product.Id);
    }
}
