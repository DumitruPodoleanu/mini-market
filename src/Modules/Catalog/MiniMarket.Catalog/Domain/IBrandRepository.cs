namespace MiniMarket.Catalog.Domain;

internal interface IBrandRepository
{
    Task<bool> ExistsAsync(Guid id);
}
