using Microsoft.EntityFrameworkCore;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Infrastructure.Persistence;
using MiniMarket.Infrastructure.Persistence;

namespace MiniMarket.Catalog.Infrastructure.Repositories;

internal sealed class SlugRepository(ICatalogDbContext dbContext)
    : Repository<SlugEntity>(dbContext), ISlugRepository
{
    public Task<SlugEntityType?> GetOwnerTypeAsync(string value, Guid? excludedSlugId, CancellationToken cancellationToken) =>
        Entities
            .Where(s => s.Value == value && s.Id != excludedSlugId)
            .Select(s => (SlugEntityType?)s.EntityType)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<string>> GetTakenWithPrefixAsync(string root, CancellationToken cancellationToken) =>
        await Entities
            .Where(s => s.Value == root || s.Value.StartsWith(root + "-"))
            .Select(s => s.Value)
            .ToListAsync(cancellationToken);
}