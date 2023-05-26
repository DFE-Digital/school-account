namespace Dfe.SchoolAccount.Web.Services.Rendering;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Models;

/// <summary>
/// Renders quotes with appropriate classes added.
/// </summary>
/// <remarks>
/// <para>This implementation is derived from https://github.com/contentful/contentful.net/blob/master/Contentful.Core/Models/Authoring.cs#L887.</para>
/// </remarks>
[ExcludeFromCodeCoverage]
public sealed class CustomQuoteRenderer : IContentRenderer
{
    private readonly ContentRendererCollection rendererCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomQuoteRenderer"/> class.
    /// </summary>
    /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
    public CustomQuoteRenderer(ContentRendererCollection rendererCollection)
    {
        this.rendererCollection = rendererCollection;
    }

    /// <inheritdoc/>
    public int Order { get; set; }

    /// <inheritdoc/>
    public bool SupportsContent(IContent content)
    {
        return content is Quote;
    }

    /// <inheritdoc/>
    public async Task<string> RenderAsync(IContent content)
    {
        var quote = (Quote)content;

        var sb = new StringBuilder();

        sb.Append($"<blockquote class=\"govuk-inset-text\">");

        foreach (var subContent in quote.Content) {
            var renderer = this.rendererCollection.GetRendererForContent(subContent);
            sb.Append(await renderer.RenderAsync(subContent));
        }

        sb.Append($"</blockquote>");

        return sb.ToString();
    }
}
