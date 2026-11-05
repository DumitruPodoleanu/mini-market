using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Shared.Domain;

namespace MiniMarket.Catalog.Domain.Repositories;

internal interface ISlugRepository : IRepository<SlugEntity>
{
    Task<SlugEntityType?> GetOwnerTypeAsync(string value, Guid? excludedSlugId, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetTakenWithPrefixAsync(string root, CancellationToken cancellationToken);
}