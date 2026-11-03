using MiniMarket.Catalog.Domain.Repositories;
using MiniMarket.Catalog.Features.Common.Errors;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common.Slugs;

internal sealed class SlugService(ISlugRepository slugRepository)
{
    private const int MinSignificantWordLength = 3;
    private const int FirstSuffix = 2;

    public async Task<Result<string>> ResolveAsync(
        string? requestedSlug, string name, Guid? currentSlugId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(requestedSlug))
        {
            var baseSlug = SlugGenerator.Normalize(name);
            if (baseSlug.Length == 0)
                return SlugErrors.CannotGenerate(name);

            return await GetAvailableAsync(baseSlug, cancellationToken);
        }

        var slug = SlugGenerator.Normalize(requestedSlug);
        if (slug.Length == 0)
            return SlugErrors.Invalid(requestedSlug);

        if (!SharesSignificantWord(slug, name))
            return SlugErrors.UnrelatedToName(slug);

        var owner = await slugRepository.GetOwnerTypeAsync(slug, currentSlugId, cancellationToken);
        if (owner is not null)
            return SlugErrors.Taken(slug, owner.Value);

        return slug;
    }

    internal static bool SharesSignificantWord(string slug, string name)
    {
        var nameWords = SlugGenerator.Normalize(name)
            .Split('-')
            .Where(word => word.Length >= MinSignificantWordLength && !word.All(char.IsDigit))
            .ToHashSet();

        if (nameWords.Count == 0)
            return true;

        return slug.Split('-').Any(nameWords.Contains);
    }

    private async Task<string> GetAvailableAsync(string baseSlug, CancellationToken cancellationToken)
    {
        var taken = await slugRepository.GetTakenWithPrefixAsync(baseSlug, cancellationToken);

        if (!taken.Contains(baseSlug))
            return baseSlug;

        var prefix = baseSlug + "-";
        var usedSuffixes = taken
            .Where(value => value.StartsWith(prefix, StringComparison.Ordinal))
            .Select(value => int.TryParse(value[prefix.Length..], out var number) ? number : 0)
            .ToList();

        var next = Math.Max(FirstSuffix, usedSuffixes.DefaultIfEmpty(0).Max() + 1);

        return $"{baseSlug}-{next}";
    }
}
