namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// Extension methods for the <see cref="IContentHyperlinkResolver"/> service.
/// </summary>
public static class ContentHyperlinkResolverExtensions
{
    /// <summary>
    /// Resolves an enumerable collection of links to some content.
    /// </summary>
    /// <remarks>
    /// <para>Unresolved or <c>null</c> entries are omitted from the resulting array.</para>
    /// </remarks>
    /// <param name="content">The content.</param>
    /// <returns>
    /// An array of zero-or-more resolved links.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentHyperlinkResolver"/> or <paramref name="content"/> is <c>null</c>.
    /// </exception>
    public static ContentHyperlink[] ResolveContentHyperlinks(this IContentHyperlinkResolver contentHyperlinkResolver, IEnumerable<object> content)
    {
        if (contentHyperlinkResolver == null) {
            throw new ArgumentNullException(nameof(contentHyperlinkResolver));
        }
        if (content == null) {
            throw new ArgumentNullException(nameof(content));
        }

        return content
            .Select(content => contentHyperlinkResolver.ResolveContentHyperlink(content))
            .Where(contentHyperlink => contentHyperlink != null)
            .Select(contentHyperlink => contentHyperlink!)
            .ToArray();
    }
}
