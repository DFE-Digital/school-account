namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches website globals content from a Contentful CDA.
/// </summary>
public sealed class ContentfulWebsiteGlobalsContentFetcher : IWebsiteGlobalsContentFetcher
{
    private readonly IContentfulClient contentfulClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulWebsiteGlobalsContentFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> is <c>null</c>.
    /// </exception>
    public ContentfulWebsiteGlobalsContentFetcher(IContentfulClient contentfulClient)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }

        this.contentfulClient = contentfulClient;
    }

    /// <inheritdoc/>
    public async Task<WebsiteGlobalsContent> FetchWebsiteGlobalsContentAsync()
    {
        var websiteGlobalsEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<WebsiteGlobalsContent>.New
                .ContentTypeIs(ContentTypeConstants.WebsiteGlobals)
        );

        return websiteGlobalsEntries.Single();
    }
}
