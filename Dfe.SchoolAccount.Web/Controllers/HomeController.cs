namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IPersonaResolver personaResolver;

    public HomeController(
        ILogger<HomeController> logger,
        IPersonaResolver personaResolver)
    {
        this.logger = logger;
        this.personaResolver = personaResolver;
    }

    [Authorize]
    [HttpGet]
    [Route("/home")]
    public IActionResult Index()
    {
        var organisation = this.User.GetOrganisation()!;

        var personaTypeName = this.personaResolver.ResolvePersona(this.User);
        Console.WriteLine("Persona type: " + personaTypeName);

        return this.View(new HomeViewModel {
            OrganisationName = organisation.Name,
        });
    }
}
