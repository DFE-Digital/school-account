namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;

public sealed class ExternalResourceContentToCardTransformHandler : IContentViewModelTransformHandler<ExternalResourceContent, CardModel>
{
    /// <inheritdoc/>
    public CardModel TransformContentToViewModel(ExternalResourceContent content)
    {
        return new CardModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = content.LinkUrl,
        };
    }
}
