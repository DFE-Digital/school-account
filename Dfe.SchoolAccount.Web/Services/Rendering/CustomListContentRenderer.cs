namespace Dfe.SchoolAccount.Web.Services.Rendering;

using Contentful.Core.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text;

/// <summary>
/// Renders list elements with appropriate classes added.
/// </summary>
/// <remarks>
/// <para>This implementation is derived from https://github.com/contentful/contentful.net/blob/master/Contentful.Core/Models/Authoring.cs#L754.</para>
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class CustomListContentRenderer : IContentRenderer
{
    private readonly ContentRendererCollection rendererCollection;

    /// <summary>
    /// Initializes a new ListContentRenderer.
    /// </summary>
    /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
    public CustomListContentRenderer(ContentRendererCollection rendererCollection)
    {
        this.rendererCollection = rendererCollection;
    }

    /// <inheritdoc/>
    public int Order { get; set; }

    /// <inheritdoc/>
    public bool SupportsContent(IContent content)
    {
        return content is List;
    }

    /// <inheritdoc/>
    public async Task<string> RenderAsync(IContent content)
    {
        var list = (List)content;

        string listTagType = "ul";
        string govukListClass = "govuk-list govuk-list--bullet govuk-list--spaced";

        if (list.NodeType == "ordered-list") {
            listTagType = "ol";
            govukListClass = "govuk-list govuk-list--number";
        }

        var sb = new StringBuilder();

        sb.Append($"<{listTagType} class=\"{govukListClass}\">");

        foreach (var subContent in list.Content) {
            var renderer = this.rendererCollection.GetRendererForContent(subContent);
            sb.Append(await renderer.RenderAsync(subContent));
        }

        sb.Append($"</{listTagType}>");

        return sb.ToString();
    }
}
