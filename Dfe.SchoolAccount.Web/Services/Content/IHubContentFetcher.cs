namespace Dfe.SchoolAccount.Web.Services.Content;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Personas;

/// <summary>
/// A service which fetches hub content.
/// </summary>
public interface IHubContentFetcher
{
    /// <summary>
    /// Fetches hub content for a given persona.
    /// </summary>
    /// <param name="personaName">Name of the persona.</param>
    /// <returns>
    /// An object with the hub content.
    /// </returns>
    /// <exception cref="UnknownPersonaException">
    /// If <paramref name="personaName"/> is <see cref="PersonaName.Unknown"/>.
    /// </exception>
    Task<HubContent> FetchHubContentAsync(PersonaName personaName);
}
