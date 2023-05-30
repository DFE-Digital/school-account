namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// Resolves hyperlinks for <see cref="SignpostingPageContent"/> references.
/// </summary>
public sealed class SignpostingPageContentHyperlinkResolutionHandler : IContentHyperlinkResolutionHandler<SignpostingPageContent>
{
    /// <inheritdoc/>
    public ContentHyperlink? ResolveContentHyperlink(SignpostingPageContent content)
    {
        return new ContentHyperlink {
            Url = $"/signposting/{content.Slug}",
            Text = content.Title,
        };
    }
}
