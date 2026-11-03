using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class ProductReferences(IBrandRepository brandRepository, ICategoryRepository categoryRepository, IVendorRepository vendorRepository)
{
    public async Task<Result<VendorEntity>> GetVendorAsync(string vendorSlug, CancellationToken cancellationToken)
    {
        var vendor = await vendorRepository.GetBySlugAsync(vendorSlug, cancellationToken);
        if (vendor is null)
            return ProductErrors.VendorNotFound(vendorSlug);

        return Result<VendorEntity>.Success(vendor);
    }

    public async Task<Result<BrandEntity>> GetBrandAsync(string brandSlug, CancellationToken cancellationToken)
    {
        var brand = await brandRepository.GetBySlugAsync(brandSlug, cancellationToken);
        if (brand is null)
            return ProductErrors.BrandNotFound(brandSlug);

        return Result<BrandEntity>.Success(brand);
    }

    public async Task<Result<IReadOnlyList<CategoryEntity>>> GetCategoriesAsync(IReadOnlyCollection<string> categorySlugs, CancellationToken cancellationToken)
    {
        var distinctSlugs = categorySlugs.Distinct().ToList();
        var categories = await categoryRepository.GetBySlugsAsync(distinctSlugs, cancellationToken);

        var missingSlugs = distinctSlugs.Except(categories.Select(c => c.Slug!.Value)).ToList();
        if (missingSlugs.Count > 0)
            return ProductErrors.CategoriesNotFound(missingSlugs);

        return Result<IReadOnlyList<CategoryEntity>>.Success(categories);
    }
}
