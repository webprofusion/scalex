using System.Text.RegularExpressions;

namespace Webprofusion.Scalex.Util
{
    public static class SlugUtility
    {
        private static readonly Regex NonSlugChars = new Regex("[^a-z0-9]+", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex DuplicateDashes = new Regex("-{2,}", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex TrailingDashes = new Regex("(^-|-$)", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex FlatNotes = new Regex("([a-g])b(?=[^a-z]|$)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static string CreateSlug(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value.Trim().ToLowerInvariant();
            normalized = normalized.Replace("♯", "sharp");
            normalized = normalized.Replace("#", "sharp");
            normalized = normalized.Replace("♭", "flat");
            normalized = FlatNotes.Replace(normalized, "$1flat");

            normalized = NonSlugChars.Replace(normalized, "-");
            normalized = DuplicateDashes.Replace(normalized, "-");
            normalized = TrailingDashes.Replace(normalized, string.Empty);

            return normalized;
        }
    }
}
