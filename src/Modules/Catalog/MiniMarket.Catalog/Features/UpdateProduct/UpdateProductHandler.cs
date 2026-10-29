using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.UpdateProduct;

internal sealed class UpdateProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    ILogger<UpdateProductHandler> logger)
{
    public async Task<Result> HandleAsync(Guid id, UpdateProductRequest request)
    {
        var product = await productRepository.GetByIdForUpdateAsync(id);
        if (product is null)
            return ProductErrors.NotFound(id);

        var brandResult = await productReferences.EnsureBrandExistsAsync(request.BrandId);
        if (!brandResult.IsSuccess)
            return brandResult.Error!;

        var categoriesResult = await productReferences.GetCategoriesAsync(request.CategoryIds ?? []);
        if (!categoriesResult.IsSuccess)
            return categoriesResult.Error!;

        request.MapTo(product, categoriesResult.Value!);

        if (await productRepository.SkuExistsAsync(product.Sku, product.Id))
            return ProductErrors.SkuAlreadyExists(product.Sku);

        await productRepository.UpdateAsync(product);

        logger.LogInformation("Product {ProductId} updated", product.Id);

        return Result.Success();
    }
}
