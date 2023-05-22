namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;

public sealed class ExternalResourceContentToCardTransformHandler : IContentViewModelTransformHandler<ExternalResourceContent, CardViewModel>
{
    /// <inheritdoc/>
    public CardViewModel TransformContentToViewModel(ExternalResourceContent content)
    {
        return new CardViewModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = content.LinkUrl,
        };
    }
}
