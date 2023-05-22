namespace Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;

public sealed class SignpostingPageContentToCardTransformHandler : IContentViewModelTransformHandler<SignpostingPageContent, CardViewModel>
{
    /// <inheritdoc/>
    public CardViewModel TransformContentToViewModel(SignpostingPageContent content)
    {
        return new CardViewModel {
            Heading = content.Title,
            Summary = content.Summary,
            LinkUrl = $"/signposting/{content.Slug}",
        };
    }
}
