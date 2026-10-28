using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal sealed class CreateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<CreateProductHandler> logger)
{
    public async Task<Guid> HandleAsync(CreateProductRequest request)
    {
        await productReferences.EnsureBrandExistsAsync(request.BrandId);
        var categories = await productReferences.GetCategoriesAsync(request.CategoryIds ?? []);

        var product = request.ToEntity(categories);

        if (await productRepository.SkuExistsAsync(product.Sku))
            throw new DomainException($"A product with SKU {product.Sku} already exists.");

        await productRepository.AddAsync(product);

        logger.LogInformation("Product {ProductId} created with SKU {Sku}", product.Id, product.Sku);

        return product.Id;
    }
}
