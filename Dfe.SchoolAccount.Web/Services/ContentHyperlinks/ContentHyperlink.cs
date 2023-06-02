namespace Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// Concrete implementation of <see cref="IContentHyperlink"/>.
/// </summary>
public sealed class ContentHyperlink : IContentHyperlink
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
