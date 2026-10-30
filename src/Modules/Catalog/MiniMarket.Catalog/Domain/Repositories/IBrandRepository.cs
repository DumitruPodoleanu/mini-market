namespace MiniMarket.Catalog.Domain.Repositories;

internal interface IBrandRepository
{
    Task<bool> ExistsAsync(Guid id);
}
