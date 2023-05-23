namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Personas;

/// <summary>
/// A service which fetches hub content from a Contentful CDA.
/// </summary>
public sealed class ContentfulHubContentFetcher : IHubContentFetcher
{
    private readonly IContentfulClient contentfulClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulHubContentFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> is <c>null</c>.
    /// </exception>
    public ContentfulHubContentFetcher(IContentfulClient contentfulClient)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }

        this.contentfulClient = contentfulClient;
    }

    /// <inheritdoc/>
    public async Task<HubContent> FetchHubContentAsync(PersonaName personaName)
    {
        if (personaName == PersonaName.Unknown) {
            throw new UnknownPersonaException();
        }

        var handleToQuery = (personaName == PersonaName.LaMaintainedSchoolUser)
            ? HubConstants.LaMaintainedSchoolUserHandle
            : HubConstants.AcademySchoolUserHandle
            ;

        var hubContentEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<HubContent>.New
                .ContentTypeIs(ContentTypeConstants.Hub)
                .FieldEquals("fields.handle", handleToQuery)
        );

        if (!hubContentEntries.Any()) {
            throw new InvalidOperationException($"Hub entry '{handleToQuery}' was not found.");
        }
        else if (hubContentEntries.Count() > 1) {
            throw new InvalidOperationException($"Multiple hub entries were found '{handleToQuery}'.");
        }

        return hubContentEntries.First();
    }
}
