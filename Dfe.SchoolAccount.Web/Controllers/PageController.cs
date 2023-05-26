namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.AspNetCore.Mvc;

public sealed class PageController : Controller
{
    private readonly ILogger<PageController> logger;
    private readonly IPageContentFetcher pageContentFetcher;

    public PageController(
        ILogger<PageController> logger,
        IPageContentFetcher pageContentFetcher)
    {
        this.logger = logger;
        this.pageContentFetcher = pageContentFetcher;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string slug)
    {
        this.logger.LogInformation($"Page '{slug}' was presented.");

        var model = await this.pageContentFetcher.FetchPageContentAsync(slug);

        if (model == null) {
            this.logger.LogError($"Could not find page with the slug '{slug}'.");
            return this.NotFound();
        }

        var view = this.View("Index", model);

        return view;
    }
}
