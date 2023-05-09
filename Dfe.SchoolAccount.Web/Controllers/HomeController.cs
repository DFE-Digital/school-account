namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.Web.Extensions;
using Dfe.SchoolAccount.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    [Authorize]
    [HttpGet]
    [Route("/home")]
    public IActionResult Index()
    {
        var organisation = this.User.GetOrganisation();

        return this.View(new HomeViewModel {
            OrganisationName = organisation.Name,
        });
    }
}
