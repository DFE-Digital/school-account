namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Microsoft.AspNetCore.Connections.Features;

/// <summary>
/// A service which fetches signposting page content from a Contentful CDA.
/// </summary>
public sealed class ContentfulSignpostingPageContentFetcher : ISignpostingPageContentFetcher
{
    private readonly IContentfulClient contentfulClient;
    private readonly IContentModelTransformer contentModelTransformer;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentfulSignpostingPageContentFetcher"/> class.
    /// </summary>
    /// <param name="contentfulClient">The contentful client.</param>
    /// <param name="contentModelTransformer">A service for transforming content models.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="contentfulClient"/> or <paramref name="contentModelTransformer"/> is <c>null</c>.
    /// </exception>
    public ContentfulSignpostingPageContentFetcher(
        IContentfulClient contentfulClient,
        IContentModelTransformer contentModelTransformer)
    {
        if (contentfulClient == null) {
            throw new ArgumentNullException(nameof(contentfulClient));
        }
        if (contentModelTransformer == null) {
            throw new ArgumentNullException(nameof(contentModelTransformer));
        }

        this.contentfulClient = contentfulClient;
        this.contentModelTransformer = contentModelTransformer;
    }

    /// <inheritdoc/>
    public async Task<SignpostingPageContent?> FetchSignpostingPageContentAsync(string slug)
    {
        if (slug == null) {
            throw new ArgumentNullException(nameof(slug));
        }
        if (slug == "") {
            throw new ArgumentException("Cannot be an empty string.", nameof(slug));
        }

        var signpostingPageEntries = await this.contentfulClient.GetEntries(
            QueryBuilder<SignpostingPageContent>.New
                .ContentTypeIs(ContentTypeConstants.SignpostingPage)
                .FieldEquals("fields.slug", slug)
        );

        if (!signpostingPageEntries.Any()) {
            return null;
        }

        var entry = signpostingPageEntries.Single();

        if (entry.Body != null) {
            this.GroupAdjacentCardsIntoCardGroups(entry.Body);
        }

        return entry;
    }

    private void GroupAdjacentCardsIntoCardGroups(Document body)
    {
        var content = new List<IContent>();
        CardGroupModel? cardGroup = null;

        for (int i = 0; i < body.Content.Count; ++i) {
            var block = body.Content[i];

            if (block is EntryStructure embeddedEntry && embeddedEntry.Data.Target is (ExternalResourceContent or SignpostingPageContent)) {
                if (cardGroup == null) {
                    cardGroup = new CardGroupModel();
                    content.Add(cardGroup);
                }
                var card = this.contentModelTransformer.TransformContentToModel<CardModel>(embeddedEntry.Data.Target);
                cardGroup.Cards.Add(card);
            }
            else {
                cardGroup = null;
                content.Add(block);
            }
        }

        body.Content = content;
    }
}
