namespace Dfe.SchoolAccount.Web.ViewComponents;

using Dfe.SchoolAccount.Web.Models.Components;
using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.AspNetCore.Mvc;

public sealed class FooterNavigationViewComponent : ViewComponent
{
    private readonly IWebsiteGlobalsFetcher websiteGlobalsFetcher;

    public FooterNavigationViewComponent(IWebsiteGlobalsFetcher websiteGlobalsFetcher)
    {
        this.websiteGlobalsFetcher = websiteGlobalsFetcher;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var websiteGlobals = await this.websiteGlobalsFetcher.FetchWebsiteGlobalsAsync();

        var model = new FooterNavigationViewModel {
            Links = websiteGlobals.FooterLinks,
        };

        return this.View(model);
    }
}
