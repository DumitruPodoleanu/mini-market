using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Features.Common.Slugs;

namespace MiniMarket.Catalog.Tests.Slugs;

public sealed class SlugGeneratorTests
{
    [Theory]
    [InlineData("Ashwagandha 60 caps", "ashwagandha-60-caps")]
    [InlineData("  Vitamina D3 — 2000 UI!  ", "vitamina-d3-2000-ui")]
    [InlineData("now-foods", "now-foods")]
    [InlineData("ALREADY--Slug--", "already-slug")]
    public void Normalize_BuildsLowercaseHyphenatedSlug(string input, string expected) =>
        Assert.Equal(expected, SlugGenerator.Normalize(input));

    [Theory]
    [InlineData("Știință și ţară", "stiinta-si-tara")]
    [InlineData("Magneziu bisglicinat și B6", "magneziu-bisglicinat-si-b6")]
    [InlineData("Магний B6", "magniy-b6")]
    [InlineData("Витамин С 1000 мг", "vitamin-s-1000-mg")]
    [InlineData("Набор Comfort Bloom", "nabor-comfort-bloom")]
    public void Normalize_TransliteratesToAscii(string input, string expected) =>
        Assert.Equal(expected, SlugGenerator.Normalize(input));

    [Theory]
    [InlineData("Nature's Way", "natures-way")]
    [InlineData("Nature’s Way", "natures-way")]
    [InlineData("CA6903/10 19.5V", "ca690310-195v")]
    public void Normalize_RemovesPunctuationInsideWords(string input, string expected) =>
        Assert.Equal(expected, SlugGenerator.Normalize(input));

    [Theory]
    [InlineData("Ashwagandha KSM-66® 120 caps", "ashwagandha-ksm-66-120-caps")]
    [InlineData("🍎 Apple", "apple")]
    [InlineData("Salt & Pepper", "salt-pepper")]
    [InlineData("5€ coupon", "5-coupon")]
    [InlineData("H₂O", "h2o")]
    public void Normalize_DropsSymbolsAndEmoji(string input, string expected) =>
        Assert.Equal(expected, SlugGenerator.Normalize(input));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("!!!")]
    [InlineData("🍎🍐")]
    [InlineData("-- / --")]
    public void Normalize_ReturnsEmpty_WhenNothingUsableRemains(string input) =>
        Assert.Equal(string.Empty, SlugGenerator.Normalize(input));

    [Fact]
    public void Normalize_TruncatesAtLastHyphen_WhenTooLong()
    {
        var input = string.Join(' ', Enumerable.Repeat("ashwagandha", 20));

        var slug = SlugGenerator.Normalize(input);

        Assert.True(slug.Length <= SlugConstraints.SlugMaxLength);
        Assert.Equal(string.Join('-', Enumerable.Repeat("ashwagandha", 12)), slug);
    }

    [Fact]
    public void Normalize_CutsHard_WhenSingleWordIsTooLong()
    {
        var input = new string('a', SlugConstraints.SlugMaxLength + 50);

        var slug = SlugGenerator.Normalize(input);

        Assert.Equal(new string('a', SlugConstraints.SlugMaxLength), slug);
    }

    [Fact]
    public void Normalize_IsIdempotent()
    {
        var once = SlugGenerator.Normalize("Magneziu & B6 — Nature’s Way®");

        Assert.Equal(once, SlugGenerator.Normalize(once));
    }
}
