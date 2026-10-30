using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.CreateProduct;

internal sealed class CreateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<CreateProductHandler> logger)
{
    public async Task<Result<Guid>> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var brandResult = await productReferences.EnsureBrandExistsAsync(request.BrandId, cancellationToken);
        if (!brandResult.IsSuccess)
            return brandResult.Error!;

        var categoriesResult = await productReferences.GetCategoriesAsync(request.CategoryIds ?? [], cancellationToken);
        if (!categoriesResult.IsSuccess)
            return categoriesResult.Error!;

        var product = request.ToEntity(categoriesResult.Value!);

        if (await productRepository.SkuExistsAsync(product.Sku, null, cancellationToken))
            return ProductErrors.SkuAlreadyExists(product.Sku);

        await productRepository.AddAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} created with SKU {Sku}", product.Id, product.Sku);

        return product.Id;
    }
}
