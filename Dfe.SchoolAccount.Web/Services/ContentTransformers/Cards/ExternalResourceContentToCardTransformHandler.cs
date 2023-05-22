namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;

public sealed class ExternalResourceContentToCardTransformHandler : IContentModelTransformHandler<ExternalResourceContent, CardModel>
{
    /// <inheritdoc/>
    public CardModel TransformContentToModel(ExternalResourceContent content)
    {
        return new CardModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = content.LinkUrl,
        };
    }
}
