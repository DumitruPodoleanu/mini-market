using Microsoft.Extensions.Logging;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common;
using MiniMarket.Catalog.Features.Common.Contracts;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Catalog.Features.Common.Localization;
using MiniMarket.Catalog.Features.Common.Mappings;
using MiniMarket.Catalog.Features.Common.Slugs;
using MiniMarket.Infrastructure.Http;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Products.UpsertProduct;

internal sealed class UpsertProductHandler(
    IProductRepository productRepository,
    ProductReferences productReferences,
    SlugService slugService,
    ILogger<UpsertProductHandler> logger)
{
    public async Task<Result<UpsertResult<ProductResponse>>> HandleAsync(
        UpsertProductCommand command,
        CancellationToken cancellationToken)
    {
        var vendorResult = await productReferences.GetVendorAsync(command.VendorSlug!, cancellationToken);
        if (!vendorResult.IsSuccess)
            return vendorResult.Error!;

        var vendor = vendorResult.Value!;
        var sku = ProductMappings.NormalizeSku(command.Sku!);

        var product = await productRepository.GetByVendorAndSkuForUpdateAsync(vendor.Id, sku, cancellationToken);

        return product is null
            ? await CreateAsync(command.Body, sku, vendor, cancellationToken)
            : await UpdateAsync(command.Body, product, cancellationToken);
    }

    private async Task<Result<UpsertResult<ProductResponse>>> CreateAsync(
        UpsertProductRequest request,
        string sku,
        VendorEntity vendor,
        CancellationToken cancellationToken)
    {
        var missingFields = new List<string>();
        if (request.Name is null) missingFields.Add(nameof(request.Name));
        if (request.Price is null) missingFields.Add(nameof(request.Price));
        if (request.BrandSlug is null) missingFields.Add(nameof(request.BrandSlug));
        if (request.CategorySlugs is null) missingFields.Add(nameof(request.CategorySlugs));

        if (missingFields.Count > 0)
            return ProductErrors.MissingFieldsForCreate(missingFields);

        var brandResult = await productReferences.GetBrandAsync(request.BrandSlug!, cancellationToken);
        if (!brandResult.IsSuccess)
            return brandResult.Error!;

        var categoriesResult = await productReferences.GetCategoriesAsync(request.CategorySlugs!, cancellationToken);
        if (!categoriesResult.IsSuccess)
            return categoriesResult.Error!;

        var slugResult = await slugService.ResolveAsync(request.Slug, request.Name!, null, cancellationToken);
        if (!slugResult.IsSuccess)
            return slugResult.Error!;

        var product = request.ToEntity(sku, slugResult.Value!, vendor, brandResult.Value!, categoriesResult.Value!);

        await productRepository.AddAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} created", product.Id);

        return new UpsertResult<ProductResponse>(product.ToResponse(RequestLanguage.Current), true);
    }

    private async Task<Result<UpsertResult<ProductResponse>>> UpdateAsync(
        UpsertProductRequest request,
        ProductEntity product,
        CancellationToken cancellationToken)
    {
        BrandEntity? brand = null;
        if (request.BrandSlug is not null)
        {
            var brandResult = await productReferences.GetBrandAsync(request.BrandSlug, cancellationToken);
            if (!brandResult.IsSuccess)
                return brandResult.Error!;

            brand = brandResult.Value;
        }

        IReadOnlyList<CategoryEntity>? categories = null;
        if (request.CategorySlugs is not null)
        {
            var categoriesResult = await productReferences.GetCategoriesAsync(request.CategorySlugs, cancellationToken);
            if (!categoriesResult.IsSuccess)
                return categoriesResult.Error!;

            categories = categoriesResult.Value;
        }

        string? slug = null;
        if (!string.IsNullOrWhiteSpace(request.Slug))
        {
            var slugResult = await slugService.ResolveAsync(
                request.Slug, request.Name ?? product.Name, product.SlugId, cancellationToken);
            if (!slugResult.IsSuccess)
                return slugResult.Error!;

            slug = slugResult.Value;
        }

        request.ApplyTo(product, brand, categories, slug);

        await productRepository.UpdateAsync(product, cancellationToken);

        logger.LogInformation("Product {ProductId} updated", product.Id);

        return new UpsertResult<ProductResponse>(product.ToResponse(RequestLanguage.Current), false);
    }
}
