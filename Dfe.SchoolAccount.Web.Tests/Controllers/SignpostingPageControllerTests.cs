namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using System.Security.Claims;
using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.Personas;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class SignpostingPageControllerTests
{
    private static SignpostingController CreateSignpostingControllerWithFakeContent(PersonaName personaName, SignpostingPageContent fakeSignpostingPage)
    {
        var logger = new NullLogger<SignpostingController>();

        var personaResolverMock = new Mock<IPersonaResolver>();
        personaResolverMock.Setup(mock => mock.ResolvePersona(It.IsAny<ClaimsPrincipal>()))
            .Returns(personaName);

        var signpostingPageContentFetcherMock = new Mock<ISignpostingPageContentFetcher>();
        signpostingPageContentFetcherMock.Setup(mock => mock.FetchSignpostingPageContentAsync(It.Is<string>(slug => slug == "an-example-slug")))
            .ReturnsAsync(fakeSignpostingPage);

        return new SignpostingController(logger, personaResolverMock.Object, signpostingPageContentFetcherMock.Object);
    }

    private static SignpostingController CreateSignpostingControllerWithFakeContent(PersonaName personaName = PersonaName.AcademySchoolUser)
    {
        var fakeSignpostingPage = new SignpostingPageContent {
            Title = "An example signposting page",
            Body = new Document {
                Content = new List<IContent> {
                    new Paragraph(),
                },
            },
            IsApplicableToAcademies = true,
            IsApplicableToLaMaintainedSchools = true,
        };

        return CreateSignpostingControllerWithFakeContent(personaName, fakeSignpostingPage);
    }

    #region Task<IActionResult> Index(string)

    [TestMethod]
    public async Task Index__ReturnsNotFound__WhenContentForSlugWasNotFound()
    {
        var signpostingPageController = CreateSignpostingControllerWithFakeContent();

        var result = await signpostingPageController.Index("a-slug-that-does-not-exist");

        TypeAssert.IsType<NotFoundResult>(result);
    }

    [DataRow(PersonaName.AcademyTrustUser, false, false)]
    [DataRow(PersonaName.AcademyTrustUser, false, true)]
    [DataRow(PersonaName.AcademySchoolUser, false, false)]
    [DataRow(PersonaName.AcademySchoolUser, false, true)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, false, false)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, true, false)]
    [DataTestMethod]
    public async Task Index__ReturnsNotFound__WhenContentIsNotApplicableToUserPersona(PersonaName personaName, bool isApplicableToAcademies, bool isApplicableToLaMaintainedSchools)
    {
        var fakeSignpostingPage = new SignpostingPageContent {
            Title = "An example signposting page",
            Body = new Document(),
            IsApplicableToAcademies = isApplicableToAcademies,
            IsApplicableToLaMaintainedSchools = isApplicableToLaMaintainedSchools,
        };

        var signpostingPageController = CreateSignpostingControllerWithFakeContent(personaName, fakeSignpostingPage);

        var result = await signpostingPageController.Index("an-example-slug");

        TypeAssert.IsType<NotFoundResult>(result);
    }

    [DataRow(PersonaName.AcademyTrustUser, true, false)]
    [DataRow(PersonaName.AcademyTrustUser, true, true)]
    [DataRow(PersonaName.AcademySchoolUser, true, false)]
    [DataRow(PersonaName.AcademySchoolUser, true, true)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, false, true)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, true, true)]
    [DataTestMethod]
    public async Task Index__ReturnsExpectedView__WhenContentIsApplicableToUserPersona(PersonaName personaName, bool isApplicableToAcademies, bool isApplicableToLaMaintainedSchools)
    {
        var fakeSignpostingPage = new SignpostingPageContent {
            Title = "An example signposting page",
            Body = new Document(),
            IsApplicableToAcademies = isApplicableToAcademies,
            IsApplicableToLaMaintainedSchools = isApplicableToLaMaintainedSchools,
        };

        var signpostingPageController = CreateSignpostingControllerWithFakeContent(personaName, fakeSignpostingPage);

        var result = await signpostingPageController.Index("an-example-slug");

        TypeAssert.IsType<ViewResult>(result);
    }

    [TestMethod]
    public async Task Index__ReturnsDefaultView()
    {
        var signpostingPageController = CreateSignpostingControllerWithFakeContent();

        var result = await signpostingPageController.Index("an-example-slug");

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithTitle()
    {
        var signpostingPageController = CreateSignpostingControllerWithFakeContent();

        var result = await signpostingPageController.Index("an-example-slug");

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var viewModel = TypeAssert.IsType<SignpostingPageViewModel>(viewResult.Model);
        Assert.AreEqual("An example signposting page", viewModel.Title);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithBodyContent()
    {
        var signpostingPageController = CreateSignpostingControllerWithFakeContent();

        var result = await signpostingPageController.Index("an-example-slug");

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var viewModel = TypeAssert.IsType<SignpostingPageViewModel>(viewResult.Model);
        Assert.IsNotNull(viewModel.Body);
        TypeAssert.IsType<Paragraph>(viewModel.Body.Content[0]);
    }

    #endregion
}
