namespace MiniMarket.Catalog.Contracts;

public interface ICatalogApi
{
    Task<ProductSummary?> GetProductBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<IReadOnlyList<ProductSummary>> GetProductsByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
}