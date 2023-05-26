namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches a general page of content.
/// </summary>
public interface IPageContentFetcher
{
    /// <summary>
    /// Fetches a general page of content with the given slug.
    /// </summary>
    /// <param name="slug">Unique slug of the page.</param>
    /// <returns>
    /// An object with the page content when found; otherwise, a value of <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="slug"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="slug"/> is an empty string.
    /// </exception>
    Task<PageContent?> FetchPageContentAsync(string slug);
}
