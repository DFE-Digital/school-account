namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

public sealed class SignpostingPageContentToCardTransformHandler : IContentModelTransformHandler<SignpostingPageContent, CardModel>
{
    private readonly IContentHyperlinkResolver contentHyperlinkResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignpostingPageContentToCardTransformHandler"/> class.
    /// </summary>
    /// <param name="contentHyperlinkResolver">A service to resolve hyperlinks from content references.</param>
    public SignpostingPageContentToCardTransformHandler(IContentHyperlinkResolver contentHyperlinkResolver)
    {
        this.contentHyperlinkResolver = contentHyperlinkResolver;
    }

    /// <inheritdoc/>
    public CardModel TransformContentToModel(SignpostingPageContent content)
    {
        var contentHyperlink = this.contentHyperlinkResolver.ResolveContentHyperlink(content);

        return new CardModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = contentHyperlink?.Url,
        };
    }
}
