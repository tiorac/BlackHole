using Microsoft.Extensions.Localization;

namespace BlackHole.Client.Locale
{
    public static class TextsHelper
    {
        public static string Get(this IStringLocalizer sl, string? key)
        {
			try
			{
                if (string.IsNullOrEmpty(key))
                    return string.Empty;

                return sl[key];
			}
			catch (Exception)
			{
                return key ?? string.Empty;
			}
        }
    }
}
