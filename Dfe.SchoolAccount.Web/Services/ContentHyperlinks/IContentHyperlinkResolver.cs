namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// A service that resolves the URL and default link text for content.
/// </summary>
public interface IContentHyperlinkResolver
{
    /// <summary>
    /// Resolves a link to some content.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// A link to the given content when applicable; otherwise, a value of <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="content"/> is <c>null</c>.
    /// </exception>
    ContentHyperlink? ResolveContentHyperlink(object content);
}
