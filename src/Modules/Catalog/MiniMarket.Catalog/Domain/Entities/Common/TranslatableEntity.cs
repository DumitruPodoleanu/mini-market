namespace MiniMarket.Catalog.Domain.Entities.Common;

internal abstract class TranslatableEntity<TTranslation> : SluggableEntity
    where TTranslation : TranslationEntity
{
    public List<TTranslation> Translations { get; set; } = [];
}