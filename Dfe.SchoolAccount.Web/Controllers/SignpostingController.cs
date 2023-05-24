namespace Dfe.SchoolAccount.Web.Controllers;

using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public sealed class SignpostingController : Controller
{
    private readonly ILogger<SignpostingController> logger;
    private readonly IPersonaResolver personaResolver;
    private readonly ISignpostingPageContentFetcher signpostingPageContentFetcher;

    public SignpostingController(
        ILogger<SignpostingController> logger,
        IPersonaResolver personaResolver,
        ISignpostingPageContentFetcher signpostingPageContentFetcher)
    {
        this.logger = logger;
        this.personaResolver = personaResolver;
        this.signpostingPageContentFetcher = signpostingPageContentFetcher;
    }

    [Authorize]
    [HttpGet]
    [Route("/signposting/{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var signpostingPageContent = await this.signpostingPageContentFetcher.FetchSignpostingPageContentAsync(slug);
        if (signpostingPageContent == null) {
            return this.NotFound();
        }

        var persona = this.personaResolver.ResolvePersona(this.User);

        bool isContentRelevant = (signpostingPageContent.IsApplicableToAcademies && persona is (PersonaName.AcademyTrustUser or PersonaName.AcademySchoolUser))
            || (signpostingPageContent.IsApplicableToLaMaintainedSchools && persona is PersonaName.LaMaintainedSchoolUser);
        if (!isContentRelevant) {
            return this.NotFound();
        }

        return this.View(new SignpostingPageViewModel {
            Title = signpostingPageContent.Title,
            Body = signpostingPageContent.Body,
        });
    }
}
