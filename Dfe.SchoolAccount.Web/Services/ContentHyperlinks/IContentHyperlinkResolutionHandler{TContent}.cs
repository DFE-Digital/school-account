namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

using Contentful.Core.Models;

/// <summary>
/// Handles URL and default link text resolution for a particular type of content.
/// </summary>
/// <typeparam name="TContent">Type of content.</typeparam>
public interface IContentHyperlinkResolutionHandler<TContent>
    where TContent : IContent
{
    /// <summary>
    /// Resolves a link to some content.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>
    /// A link to the given content when applicable; otherwise, a value of <c>null</c>.
    /// </returns>
    ContentHyperlink? ResolveContentHyperlink(TContent content);
}
