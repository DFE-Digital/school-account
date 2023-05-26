namespace Dfe.SchoolAccount.Web.Services.Rendering;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Models;

/// <summary>
/// Renders headings starting at <c>&lt;h2&gt;</c> with appropriate classes added.
/// </summary>
/// <remarks>
/// <para>This implementation is derived from https://github.com/contentful/contentful.net/blob/master/Contentful.Core/Models/Authoring.cs#L430.</para>
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class CustomHeadingRenderer : IContentRenderer
{
    private readonly ContentRendererCollection rendererCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomHeadingRenderer"/> class.
    /// </summary>
    /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
    public CustomHeadingRenderer(ContentRendererCollection rendererCollection)
    {
        this.rendererCollection = rendererCollection;
    }

    /// <inheritdoc/>
    public int Order { get; set; }

    /// <inheritdoc/>
    public bool SupportsContent(IContent content)
    {
        return content is (Heading1 or Heading2 or Heading3 or Heading4 or Heading5 or Heading6);
    }

    /// <inheritdoc/>
    public async Task<string> RenderAsync(IContent content)
    {
        var heading = (IHeading)content;

        switch (content) {
        case Heading1:
            return await this.RenderHeadingAsync(heading, "h2", "govuk-heading-l");
        case Heading2:
            return await this.RenderHeadingAsync(heading, "h3", "govuk-heading-m");
        case Heading3:
            return await this.RenderHeadingAsync(heading, "h4", "govuk-heading-s");
        case Heading4:
            return await this.RenderHeadingAsync(heading, "h5", "");
        case Heading5:
            return await this.RenderHeadingAsync(heading, "h6", "");
        }

        return "";
    }

    private async Task<string> RenderHeadingAsync(IHeading heading, string elementName, string classes)
    {
        var sb = new StringBuilder();
        sb.Append($"<{elementName} class=\"{classes}\">");

        await this.RenderContents(heading, sb);

        sb.Append($"</{elementName}>");
        return sb.ToString();

    }

    private async Task RenderContents(IHeading heading, StringBuilder sb)
    {
        foreach (var subContent in heading.Content) {
            var renderer = this.rendererCollection.GetRendererForContent(subContent);
            sb.Append(await renderer.RenderAsync(subContent));
        }
    }
}
