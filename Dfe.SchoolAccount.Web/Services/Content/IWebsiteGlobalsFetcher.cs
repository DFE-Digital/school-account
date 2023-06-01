namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches website globals.
/// </summary>
public interface IWebsiteGlobalsFetcher
{
    /// <summary>
    /// Fetches website globals.
    /// </summary>
    /// <returns>
    /// An object with the website globals.
    /// </returns>
    Task<WebsiteGlobalsModel> FetchWebsiteGlobalsAsync();
}
