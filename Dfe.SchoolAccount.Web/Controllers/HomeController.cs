namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IPersonaResolver personaResolver;
    private readonly IHubContentFetcher hubContentFetcher;
    private readonly IContentModelTransformer contentModelTransformer;

    public HomeController(
        ILogger<HomeController> logger,
        IPersonaResolver personaResolver,
        IHubContentFetcher hubContentFetcher,
        IContentModelTransformer contentViewModelTransformer)
    {
        this.logger = logger;
        this.personaResolver = personaResolver;
        this.hubContentFetcher = hubContentFetcher;
        this.contentModelTransformer = contentViewModelTransformer;
    }

    [Authorize]
    [HttpGet]
    [Route("/home")]
    public async Task<IActionResult> Index()
    {
        var organisation = this.User.GetOrganisation()!;

        var persona = this.personaResolver.ResolvePersona(this.User);
        var hubContent = await this.hubContentFetcher.FetchHubContentAsync(persona);

        return this.View(new HomeViewModel {
            OrganisationName = organisation.Name,
            UsefulServicesAndGuidanceCards = this.contentModelTransformer.TransformContentToModel<CardModel>(hubContent.UsefulServicesAndGuidanceCards),
            SupportCards = this.contentModelTransformer.TransformContentToModel<CardModel>(hubContent.SupportCards),
        });
    }
}
