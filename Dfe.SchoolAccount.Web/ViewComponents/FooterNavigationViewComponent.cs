namespace Dfe.SchoolAccount.Web.ViewComponents;

using Dfe.SchoolAccount.Web.Models.Components;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Microsoft.AspNetCore.Mvc;

public sealed class FooterNavigationViewComponent : ViewComponent
{
    private readonly IWebsiteGlobalsContentFetcher websiteGlobalsContentFetcher;
    private readonly IContentHyperlinkResolver contentHyperlinkResolver;

    public FooterNavigationViewComponent(
        IWebsiteGlobalsContentFetcher websiteGlobalsContentFetcher,
        IContentHyperlinkResolver contentHyperlinkResolver)
    {
        this.websiteGlobalsContentFetcher = websiteGlobalsContentFetcher;
        this.contentHyperlinkResolver = contentHyperlinkResolver;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var websiteGlobals = await this.websiteGlobalsContentFetcher.FetchWebsiteGlobalsContentAsync();

        var model = new FooterNavigationViewModel {
            Links = this.contentHyperlinkResolver.ResolveContentHyperlinks(websiteGlobals.FooterLinks),
        };

        return this.View(model);
    }
}
