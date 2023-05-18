namespace Dfe.SchoolAccount.Web.Helpers;

using System.Text.RegularExpressions;

/// <summary>
/// Helper functionality for use in views.
/// </summary>
public static class ViewHelpers
{
    private static readonly Regex s_TrimSlugCharacterRegex = new Regex(@"[A-Z0-9](.*[A-Z0-9]+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex s_InvalidSlugCharacterRegex = new Regex(@"[^A-Z0-9]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Arranges an enumerable collection of items into groups of a given quantity.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    /// <param name="items">The full collection of items.</param>
    /// <param name="itemsPerGroup">The maximum number of items per group.</param>
    /// <returns>
    /// A list of grouped items.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="items"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If <paramref name="itemsPerGroup"/> is less than 1.
    /// </exception>
    public static IReadOnlyList<IReadOnlyList<T>> ArrangeIntoGroupsOf<T>(this IEnumerable<T> items, int itemsPerGroup)
    {
        if (items == null) {
            throw new ArgumentNullException(nameof(items));
        }
        if (itemsPerGroup <= 0) {
            throw new ArgumentOutOfRangeException(nameof(itemsPerGroup));
        }

        return items.Select((x, idx) => new { x, idx })
            .GroupBy(x => x.idx / itemsPerGroup)
            .Select(g => g.Select(a => a.x).ToList())
            .ToList();
    }

    /// <summary>
    /// Generate a unique identifier for the purpose of relating HTML elements.
    /// </summary>
    /// <returns>
    /// A unique identifier that is compatible with the <c>id</c> attribute of a HTML element.
    /// </returns>
    public static string GetUniqueIdentifier()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Convert title string to a format which could be used as an identifier.
    /// </summary>
    /// <param name="title">Title text.</param>
    /// <returns>
    /// A string which contains only lower-case characters, digits, underscores, and hyphens.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="title"/> is <c>null</c>.
    /// </exception>
    public static string ConvertTitleToSlug(string title)
    {
        if (title == null) {
            throw new ArgumentNullException(nameof(title));
        }

        string slug = title.ToLower();
        slug = s_TrimSlugCharacterRegex.Match(slug).Value;
        return s_InvalidSlugCharacterRegex.Replace(slug, "-");
    }
}
