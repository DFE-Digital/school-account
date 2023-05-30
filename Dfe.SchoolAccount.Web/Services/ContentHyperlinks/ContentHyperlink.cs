namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// Represents a link to some content.
/// </summary>
/// <seealso cref="IContentHyperlinkResolver"/>
/// <seealso cref="IContentHyperlinkResolutionHandler{TContent}"/>
public sealed class ContentHyperlink
{
    /// <summary>
    /// Gets or sets the URL of the link.
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets the text of the link.
    /// </summary>
    public string Text { get; set; } = null!;
}
