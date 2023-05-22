namespace Dfe.SchoolAccount.Web.Services.Rendering;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Contentful.AspNetCore.Authoring;
using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Models.Partials;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

/// <summary>
/// A renderer which adds support for rendering <see cref="CardGroupModel"/> with
/// Contentful's rendering mechanism.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CardGroupModelRenderer : RazorContentRenderer
{
    public CardGroupModelRenderer(IRazorViewEngine razorViewEngine,  ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        : base(razorViewEngine, tempDataProvider, serviceProvider)
    {
    }

    /// <inheritdoc/>
    public override bool SupportsContent(IContent content)
    {
        return content is CardGroupModel;
    }

    /// <inheritdoc/>
    public override string Render(IContent content)
    {
        // This seems to be an obsolete method which is no longer being used.
        // See interface `IContentRenderer.RenderAsync(IContent)`.
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override Task<string> RenderAsync(IContent content)
    {
        var model = (CardGroupModel)content;

        return this.RenderToString("Partials/_Cards", new CardsPartialViewModel {
            Cards = model.Cards,
        });
    }
}
