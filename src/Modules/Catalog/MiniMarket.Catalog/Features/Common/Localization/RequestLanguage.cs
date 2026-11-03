using System.Globalization;

namespace MiniMarket.Catalog.Features.Common.Localization;

internal static class RequestLanguage
{
    public static string Current => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
}
