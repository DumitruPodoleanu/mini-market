namespace MiniMarket.Catalog.Domain;

internal interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetByIdsAsync(IReadOnlyCollection<Guid> ids);
}
