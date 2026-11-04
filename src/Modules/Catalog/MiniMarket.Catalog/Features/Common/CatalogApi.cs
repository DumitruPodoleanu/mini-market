using MiniMarket.Catalog.Contracts;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Localization;
using MiniMarket.Catalog.Features.Common.Mappings;

namespace MiniMarket.Catalog.Features.Common;

internal sealed class CatalogApi(IProductRepository productRepository) : ICatalogApi
{
    public async Task<ProductSummary?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetBySlugAsync(slug, cancellationToken);

        return product?.ToSummary(RequestLanguage.Current);
    }

    public async Task<IReadOnlyList<ProductSummary>> GetProductsByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetByIdsAsync(ids, cancellationToken);

        return products.Select(product => product.ToSummary(RequestLanguage.Current)).ToList();
    }

}