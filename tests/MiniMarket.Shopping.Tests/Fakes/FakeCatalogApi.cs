using MiniMarket.Catalog.Contracts;

namespace MiniMarket.Shopping.Tests.Fakes;

internal sealed class FakeCatalogApi(params ProductSummary[] products) : ICatalogApi
{
    public Task<ProductSummary?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken) =>
        Task.FromResult(products.FirstOrDefault(product => product.Slug == slug));

    public Task<IReadOnlyList<ProductSummary>> GetProductsByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
        Task.FromResult<IReadOnlyList<ProductSummary>>(products.Where(product => ids.Contains(product.Id)).ToList());
}
