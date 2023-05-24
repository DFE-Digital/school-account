namespace Dfe.SchoolAccount.Web.Controllers;

using System.Diagnostics;
using Dfe.SchoolAccount.Web.Models;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorController : Controller
{
    private readonly ILogger<ErrorController> logger;

    public ErrorController(
        ILogger<ErrorController> logger)
    {
        this.logger = logger;
    }
    
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(int? statusCode = null)
    {
        ViewResult view;

        var model = new ErrorViewModel {
            RequestId = this.HttpContext?.TraceIdentifier,
        };

        if (statusCode == 403) {
            view = this.View("Restricted", model);
        }
        else if (statusCode == 404) {
            view = this.View("NotFound", model);
        }
        else {
            view = this.View(model);
        }

        view.StatusCode = statusCode ?? 500;

        return view;
    }
}
