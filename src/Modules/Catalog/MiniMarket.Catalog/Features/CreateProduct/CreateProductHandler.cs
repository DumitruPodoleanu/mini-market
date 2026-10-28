using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.CreateProduct;

internal sealed class CreateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<CreateProductHandler> logger)
{
    public async Task<Result<Guid>> HandleAsync(CreateProductRequest request)
    {
        var brandResult = await productReferences.EnsureBrandExistsAsync(request.BrandId);
        if (!brandResult.IsSuccess)
            return brandResult.Error!;

        var categoriesResult = await productReferences.GetCategoriesAsync(request.CategoryIds ?? []);
        if (!categoriesResult.IsSuccess)
            return categoriesResult.Error!;

        var product = request.ToEntity(categoriesResult.Value!);

        if (await productRepository.SkuExistsAsync(product.Sku))
            return ProductErrors.SkuAlreadyExists(product.Sku);

        await productRepository.AddAsync(product);

        logger.LogInformation("Product {ProductId} created with SKU {Sku}", product.Id, product.Sku);

        return product.Id;
    }
}
