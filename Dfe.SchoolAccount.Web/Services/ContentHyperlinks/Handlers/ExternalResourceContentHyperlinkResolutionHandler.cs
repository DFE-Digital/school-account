namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;

/// <summary>
/// Resolves hyperlinks for <see cref="ExternalResourceContent"/> references.
/// </summary>
public sealed class ExternalResourceContentHyperlinkResolutionHandler : IContentHyperlinkResolutionHandler<ExternalResourceContent>
{
    /// <inheritdoc/>
    public ContentHyperlink? ResolveContentHyperlink(ExternalResourceContent content)
    {
        return new ContentHyperlink {
            Url = content.LinkUrl,
            Text = content.Title,
        };
    }
}
