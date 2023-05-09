namespace Dfe.SchoolAccount.Web.Controllers;

using System.Diagnostics;
using Dfe.SchoolAccount.Web.Models;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorController : Controller
{
    private readonly ILogger<ErrorController> logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        this.logger = logger;
    }
    
    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(int? statusCode = null)
    {
        ViewResult view;

        if (statusCode == 404) {
            view = this.View("NotFound");
        }
        else {
            view = this.View(new ErrorViewModel {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            });
        }

        view.StatusCode = statusCode ?? 500;

        return view;
    }
}
