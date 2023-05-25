namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// A service which fetches signposting page content from a Contentful CDA.
/// </summary>
public sealed class ContentfulErrorPageContentFetcher : IErrorPageContentFetcher
{
    private readonly IContentfulClient contentfulClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulErrorPageContentFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> is <c>null</c>.
    /// </exception>
    public ContentfulErrorPageContentFetcher(IContentfulClient contentfulClient)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }

        this.contentfulClient = contentfulClient;
    }

    /// <inheritdoc/>
    public async Task<ErrorPageContent?> FetchErrorPageContentAsync(string handle)
    {
        if (handle == null) {
            throw new ArgumentNullException(nameof(handle));
        }
        if (handle == "") {
            throw new ArgumentException("Cannot be an empty string.", nameof(handle));
        }

        var errorPageEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<ErrorPageContent>.New
                .ContentTypeIs(ContentTypeConstants.ErrorPage)
                .FieldEquals("fields.handle", handle)
        );

        return errorPageEntries.SingleOrDefault();
    }
}
