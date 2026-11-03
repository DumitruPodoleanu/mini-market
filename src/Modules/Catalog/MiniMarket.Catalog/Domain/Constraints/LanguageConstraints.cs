namespace MiniMarket.Catalog.Domain.Constraints;

internal static class LanguageConstraints
{
    public const string Romanian = "ro";
    public const string Russian = "ru";
    public const string Default = Romanian;
    public const int CodeLength = 2;
    public static readonly string[] Supported = [Romanian, Russian];
}
