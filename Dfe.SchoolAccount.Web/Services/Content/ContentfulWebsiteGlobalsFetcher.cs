namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// A service which fetches website globals from a Contentful CDA.
/// </summary>
public sealed class ContentfulWebsiteGlobalsFetcher : IWebsiteGlobalsFetcher
{
    private readonly IContentfulClient contentfulClient;
    private readonly IContentHyperlinkResolver contentHyperlinkResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulWebsiteGlobalsFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> or <see cref="contentHyperlinkResolver"/> is <c>null</c>.
    /// </exception>
    public ContentfulWebsiteGlobalsFetcher(
        IContentfulClient contentfulClient,
        IContentHyperlinkResolver contentHyperlinkResolver)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }
        if (contentHyperlinkResolver == null) {
            throw new ArgumentNullException(nameof(contentHyperlinkResolver));
        }

        this.contentfulClient = contentfulClient;
        this.contentHyperlinkResolver = contentHyperlinkResolver;
    }

    /// <inheritdoc/>
    public async Task<IWebsiteGlobalsModel> FetchWebsiteGlobalsAsync()
    {
        var websiteGlobalsEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<WebsiteGlobalsContent>.New
                .ContentTypeIs(ContentTypeConstants.WebsiteGlobals)
        );

        var websiteGlobalsContent = websiteGlobalsEntries.Single();

        return new WebsiteGlobalsModel {
            SiteTitle = websiteGlobalsContent.SiteTitle,
            FooterLinks = this.contentHyperlinkResolver.ResolveContentHyperlinks(websiteGlobalsContent.FooterLinks),
        };
    }
}
