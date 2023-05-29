namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches general page content from a Contentful CDA.
/// </summary>
public sealed class ContentfulPageContentFetcher : IPageContentFetcher
{
    private readonly IContentfulClient contentfulClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulPageContentFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> is <c>null</c>.
    /// </exception>
    public ContentfulPageContentFetcher(IContentfulClient contentfulClient)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }

        this.contentfulClient = contentfulClient;
    }

    /// <inheritdoc/>
    public async Task<PageContent?> FetchPageContentAsync(string slug)
    {
        if (slug == null) {
            throw new ArgumentNullException(nameof(slug));
        }
        if (slug == "") {
            throw new ArgumentException("Cannot be an empty string.", nameof(slug));
        }

        var pageEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<PageContent>.New
                .ContentTypeIs(ContentTypeConstants.Page)
                .FieldEquals("fields.slug", slug)
        );

        return pageEntries.SingleOrDefault();
    }
}
