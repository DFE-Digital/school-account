namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// Represents a link to some content.
/// </summary>
/// <seealso cref="IContentHyperlinkResolver"/>
/// <seealso cref="IContentHyperlinkResolutionHandler{TContent}"/>
public interface IContentHyperlink
{
    /// <summary>
    /// Gets the URL of the link.
    /// </summary>
    string Text { get; set; }

    /// <summary>
    /// Gets the text of the link.
    /// </summary>
    string Url { get; set; }
}
