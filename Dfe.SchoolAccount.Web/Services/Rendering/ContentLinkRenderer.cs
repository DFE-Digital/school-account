namespace Dfe.SchoolAccount.Web.Services.Rendering;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;

/// <summary>
/// Renders hyperlinks to content.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ContentLinkRenderer : IContentRenderer
{
    private readonly IContentHyperlinkResolver contentHyperlinkResolver;
    private readonly ContentRendererCollection rendererCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentLinkRenderer"/> class.
    /// </summary>
    /// <param name="contentHyperlinkResolver">A service to resolve hyperlinks from content references.</param>
    /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
    public ContentLinkRenderer(IContentHyperlinkResolver contentHyperlinkResolver, ContentRendererCollection rendererCollection)
    {
        this.contentHyperlinkResolver = contentHyperlinkResolver;
        this.rendererCollection = rendererCollection;
    }

    /// <inheritdoc/>
    public int Order { get; set; }

    /// <inheritdoc/>
    public bool SupportsContent(IContent content)
    {
        return content is EntryStructure entryStructure && entryStructure.Data is EntryStructureData && entryStructure.NodeType == "entry-hyperlink";
    }

    /// <inheritdoc/>
    public async Task<string> RenderAsync(IContent content)
    {
        var entryStructure = (EntryStructure)content;

        var contentHyperlink = this.contentHyperlinkResolver.ResolveContentHyperlink(entryStructure.Data.Target);
        if (contentHyperlink == null) {
            return "[missing link]";
        }

        string innerContent = await this.RenderInnerContent(entryStructure);
        if (string.IsNullOrWhiteSpace(innerContent)) {
            innerContent = contentHyperlink.Text;
        }

        return $"<a href=\"{contentHyperlink.Url}\">{innerContent}</a>";
    }

    private async Task<string> RenderInnerContent(EntryStructure entryStructure)
    {
        var sb = new StringBuilder();

        foreach (var subContent in entryStructure.Content) {
            var renderer = this.rendererCollection.GetRendererForContent(subContent);
            sb.Append(await renderer.RenderAsync(subContent));
        }

        return sb.ToString();
    }
}
