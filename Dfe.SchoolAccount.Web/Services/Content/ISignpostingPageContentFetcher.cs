namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches signposting page content.
/// </summary>
public interface ISignpostingPageContentFetcher
{
    /// <summary>
    /// Fetches hub content for a given persona.
    /// </summary>
    /// <param name="slug">Slug of the signposting page.</param>
    /// <returns>
    /// An object with the signposting page content when found; otherwise, a value of
    /// <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="slug"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="slug"/> is an empty string.
    /// </exception>
    Task<SignpostingPageContent?> FetchSignpostingPageContentAsync(string slug);
}
