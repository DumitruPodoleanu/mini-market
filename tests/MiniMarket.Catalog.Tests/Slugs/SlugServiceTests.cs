using MiniMarket.Catalog.Features.Common.Slugs;

namespace MiniMarket.Catalog.Tests.Slugs;

public sealed class SlugServiceTests
{
    [Theory]
    [InlineData("ashwagandha-ksm-66", "Ashwagandha 60 caps")]
    [InlineData("magniy-b6", "Магний B6 100 таблеток")]
    [InlineData("vitamina-d3-solgar", "Vitamina D3 2000 UI 120 caps")]
    [InlineData("b6-60", "B6 60")]
    public void SharesSignificantWord_ReturnsTrue_WhenSlugMatchesName(string slug, string name) =>
        Assert.True(SlugService.SharesSignificantWord(slug, name));

    [Theory]
    [InlineData("samsung-galaxy-s21", "iPhone 17 Pro")]
    [InlineData("iphone-17", "Samsung Galaxy S21")]
    [InlineData("b6-60-ab", "Solgar Vitamin C")]
    public void SharesSignificantWord_ReturnsFalse_WhenSlugIsUnrelated(string slug, string name) =>
        Assert.False(SlugService.SharesSignificantWord(slug, name));
}
