using MiniMarket.Catalog.Domain;
using MiniMarket.Shared;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class ProductReferences(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
{
    public async Task EnsureBrandExistsAsync(Guid brandId)
    {
        if (!await brandRepository.ExistsAsync(brandId))
            throw new DomainException($"Brand {brandId} does not exist.");
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(IReadOnlyCollection<Guid> categoryIds)
    {
        var distinctIds = categoryIds.Distinct().ToList();
        var categories = await categoryRepository.GetByIdsAsync(distinctIds);

        var missingIds = distinctIds.Except(categories.Select(c => c.Id)).ToList();
        if (missingIds.Count > 0)
            throw new DomainException($"Categories do not exist: {string.Join(", ", missingIds)}.");

        return categories;
    }
}
