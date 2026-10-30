using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class ProductReferences(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
{
    public async Task<Result> EnsureBrandExistsAsync(Guid brandId, CancellationToken cancellationToken)
    {
        if (!await brandRepository.ExistsAsync(brandId, cancellationToken))
            return ProductErrors.BrandNotFound(brandId);

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<CategoryEntity>>> GetCategoriesAsync(IReadOnlyCollection<Guid> categoryIds, CancellationToken cancellationToken)
    {
        var distinctIds = categoryIds.Distinct().ToList();
        var categories = await categoryRepository.GetByIdsAsync(distinctIds, cancellationToken);

        var missingIds = distinctIds.Except(categories.Select(c => c.Id)).ToList();
        if (missingIds.Count > 0)
            return ProductErrors.CategoriesNotFound(missingIds);

        return Result<IReadOnlyList<CategoryEntity>>.Success(categories);
    }
}
