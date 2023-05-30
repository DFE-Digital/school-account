namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// Resolves hyperlinks for <see cref="PageContent"/> references.
/// </summary>
public sealed class PageContentHyperlinkResolutionHandler : IContentHyperlinkResolutionHandler<PageContent>
{
    /// <inheritdoc/>
    public ContentHyperlink? ResolveContentHyperlink(PageContent content)
    {
        return new ContentHyperlink {
            Url = $"/{content.Slug}",
            Text = content.Title,
        };
    }
}
