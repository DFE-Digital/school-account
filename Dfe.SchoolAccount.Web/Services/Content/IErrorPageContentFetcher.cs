namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches error page content.
/// </summary>
public interface IErrorPageContentFetcher
{
    /// <summary>
    /// Fetches error page content with the given handle.
    /// </summary>
    /// <param name="handle">Unique handle of the error page.</param>
    /// <returns>
    /// An object with the error page content when found; otherwise, a value of
    /// <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="handle"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="handle"/> is an empty string.
    /// </exception>
    Task<ErrorPageContent?> FetchErrorPageContentAsync(string handle);
}
