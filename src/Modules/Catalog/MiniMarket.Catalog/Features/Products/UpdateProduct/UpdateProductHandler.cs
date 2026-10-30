using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.UpdateProduct;

internal sealed class UpdateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<UpdateProductHandler> logger)
{
    public async Task<Result> HandleAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdForUpdateAsync(id, cancellationToken);
        if (product is null)
            return ProductErrors.NotFound(id);

        var brandResult = await productReferences.EnsureBrandExistsAsync(request.BrandId, cancellationToken);
        if (!brandResult.IsSuccess)
            return brandResult.Error!;

        var categoriesResult = await productReferences.GetCategoriesAsync(request.CategoryIds ?? [], cancellationToken);
        if (!categoriesResult.IsSuccess)
            return categoriesResult.Error!;

        request.MapTo(product, categoriesResult.Value!);

        if (await productRepository.SkuExistsAsync(product.Sku, product.Id, cancellationToken))
            return ProductErrors.SkuAlreadyExists(product.Sku);

        await productRepository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} updated", product.Id);

        return Result.Success();
    }
}
