namespace Dfe.SchoolAccount.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

public sealed class StartController : Controller
{
    private readonly ILogger<StartController> logger;

    public StartController(
        ILogger<StartController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    [Route("/start")]
    public IActionResult Index()
    {
        return this.View();
    }
}
