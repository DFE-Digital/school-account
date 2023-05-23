namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;

public sealed class SignpostingPageContentToCardTransformHandler : IContentModelTransformHandler<SignpostingPageContent, CardModel>
{
    /// <inheritdoc/>
    public CardModel TransformContentToModel(SignpostingPageContent content)
    {
        return new CardModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = $"/signposting/{content.Slug}",
        };
    }
}
