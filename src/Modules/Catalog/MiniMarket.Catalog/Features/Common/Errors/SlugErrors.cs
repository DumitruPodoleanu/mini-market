using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Slugs;
using MiniMarket.Shared.Results;

namespace MiniMarket.Catalog.Features.Common.Errors;

internal static class SlugErrors
{
    public static Error Taken(string slug, SlugEntityType owner) =>
        new($"Slug '{slug}' is already used by a {owner.ToString().ToLowerInvariant()}.", ErrorType.Conflict);

    public static Error UnrelatedToName(string slug) =>
        new($"Slug '{slug}' must share at least one word with the name.", ErrorType.Validation);

    public static Error CannotGenerate(string name) =>
        new($"A slug cannot be generated from name '{name}'.", ErrorType.Validation);

    public static Error Invalid(string requested) =>
        new($"Slug '{requested}' contains no letters or digits.", ErrorType.Validation);
}