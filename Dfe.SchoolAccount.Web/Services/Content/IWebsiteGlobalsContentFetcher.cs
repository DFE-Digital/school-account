namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches website globals content.
/// </summary>
public interface IWebsiteGlobalsContentFetcher
{
    /// <summary>
    /// Fetches website globals content.
    /// </summary>
    /// <returns>
    /// An object with the website globals content.
    /// </returns>
    Task<WebsiteGlobalsContent> FetchWebsiteGlobalsContentAsync();
}
