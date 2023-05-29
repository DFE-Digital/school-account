namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorPageController : Controller
{
    private readonly ILogger<ErrorPageController> logger;
    private readonly IErrorPageContentFetcher errorPageContentFetcher;

    public ErrorPageController(
        ILogger<ErrorPageController> logger,
        IErrorPageContentFetcher errorPageContentFetcher)
    {
        this.logger = logger;
        this.errorPageContentFetcher = errorPageContentFetcher;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string handle, int statusCode)
    {
        this.logger.LogInformation($"Error page '{handle}' was presented.");

        var model = await this.errorPageContentFetcher.FetchErrorPageContentAsync(handle);

        if (model == null) {
            this.logger.LogError($"Could not find error page with the handle '{handle}'.");
            return this.NotFound();
        }

        var view = this.View("Index", model);
        view.StatusCode = statusCode;

        return view;
    }
}
