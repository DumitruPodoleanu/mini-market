using System.Text.RegularExpressions;
using AnyAscii;
using MiniMarket.Catalog.Domain.Constraints;

namespace MiniMarket.Catalog.Features.Common.Slugs;

internal static partial class SlugGenerator
{
    public static string Normalize(string input)
    {
        var value = Symbols().Replace(input, " ");
        value = value.Transliterate().ToLowerInvariant();
        value = InWordPunctuation().Replace(value, "");
        value = Separators().Replace(value, "-");
        value = value.Trim('-');
        return Truncate(value);
    }

    private static string Truncate(string slug)
    {
        if (slug.Length <= SlugConstraints.SlugMaxLength)
            return slug;

        var cut = slug[..SlugConstraints.SlugMaxLength];
        var lastSeparator = cut.LastIndexOf('-');

        return (lastSeparator > 0 ? cut[..lastSeparator] : cut).Trim('-');
    }

    [GeneratedRegex(@"[\p{S}\p{Cs}]")]
    private static partial Regex Symbols();

    [GeneratedRegex(@"['’./]")]
    private static partial Regex InWordPunctuation();

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex Separators();
}