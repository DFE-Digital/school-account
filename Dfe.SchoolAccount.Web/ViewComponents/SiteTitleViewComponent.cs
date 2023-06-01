namespace Dfe.SchoolAccount.Web.ViewComponents;

using Dfe.SchoolAccount.Web.Models.Components;
using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.AspNetCore.Mvc;

public sealed class SiteTitleViewComponent : ViewComponent
{
    private readonly IWebsiteGlobalsFetcher websiteGlobalsContentFetcher;

    public SiteTitleViewComponent(
        IWebsiteGlobalsFetcher websiteGlobalsContentFetcher)
    {
        this.websiteGlobalsContentFetcher = websiteGlobalsContentFetcher;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var websiteGlobals = await this.websiteGlobalsContentFetcher.FetchWebsiteGlobalsAsync();

        var model = new SiteTitleViewModel {
            SiteTitle = websiteGlobals.SiteTitle,
        };

        return this.View(model);
    }
}
