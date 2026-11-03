using MiniMarket.Catalog.Domain.Constraints;
using MiniMarket.Catalog.Domain.Entities;
using MiniMarket.Catalog.Domain.Entities.Translations;
using MiniMarket.Catalog.Features.Common.Localization;

namespace MiniMarket.Catalog.Tests.Localization;

public sealed class TranslationExtensionsTests
{
    private static CategoryEntity Category(params CategoryTranslationEntity[] translations) =>
        new() { Name = "Căști", Translations = [.. translations] };

    [Fact]
    public void Translate_ReturnsTheTranslationForTheLanguage()
    {
        var category = Category(new CategoryTranslationEntity { Language = LanguageConstraints.Russian, Name = "Наушники" });

        Assert.Equal("Наушники", category.Translate(LanguageConstraints.Russian, t => t.Name, category.Name));
    }

    [Theory]
    [InlineData(LanguageConstraints.Romanian)]
    [InlineData("en")]
    public void Translate_FallsBackWhenTheLanguageHasNoTranslation(string language)
    {
        var category = Category(new CategoryTranslationEntity { Language = LanguageConstraints.Russian, Name = "Наушники" });

        Assert.Equal("Căști", category.Translate(language, t => t.Name, category.Name));
    }

    [Fact]
    public void Translate_FallsBackWhenTheFieldIsNotTranslated()
    {
        var category = Category(new CategoryTranslationEntity { Language = LanguageConstraints.Russian, Name = null });

        Assert.Equal("Căști", category.Translate(LanguageConstraints.Russian, t => t.Name, category.Name));
    }

    [Fact]
    public void Translate_FallsBackWhenThereAreNoTranslations()
    {
        var category = Category();

        Assert.Equal("Căști", category.Translate(LanguageConstraints.Russian, t => t.Name, category.Name));
    }
}
