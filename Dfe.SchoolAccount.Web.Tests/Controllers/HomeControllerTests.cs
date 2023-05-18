namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.Personas;
using Dfe.SchoolAccount.Web.Tests.Fakes;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class HomeControllerTests
{
    private static HomeController CreateHomeControllerWithCommunitySchoolUser(IHubContentFetcher hubContentFetcher)
    {
        var logger = new NullLogger<HomeController>();

        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();
        var personaResolver = new OrganisationTypePersonaResolver();

        return new HomeController(logger, personaResolver, hubContentFetcher) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext {
                    User = fakeUser,
                },
            }
        };
    }

    #region Task<IActionResult> Index()

    [TestMethod]
    public async Task Index__ReturnsDefaultView()
    {
        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent());

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithOrganisationName()
    {
        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent());

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual("An example organisation name", model.OrganisationName);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithUsefulServicesAndGuidanceCards()
    {
        var firstTestCardContent = new CardContent {
            Heading = "First test card",
            LinkUrl = "https://example.localhost/first-test-card",
            Summary = "Summary text for the first test card.",
        };
        var secondTestCardContent = new CardContent {
            Heading = "Second test card",
            LinkUrl = "https://example.localhost/second-test-card",
            Summary = "Summary text for the second test card.",
        };

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                UsefulServicesAndGuidanceCards = new CardContent[] {
                    firstTestCardContent,
                    secondTestCardContent,
                },
            });

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual(2, model.UsefulServicesAndGuidanceCards.Count);
        Assert.AreSame(firstTestCardContent, model.UsefulServicesAndGuidanceCards[0]);
        Assert.AreSame(secondTestCardContent, model.UsefulServicesAndGuidanceCards[1]);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithSupportCards()
    {
        var firstTestCardContent = new CardContent {
            Heading = "First test card",
            LinkUrl = "https://example.localhost/first-test-card",
            Summary = "Summary text for the first test card.",
        };
        var secondTestCardContent = new CardContent {
            Heading = "Second test card",
            LinkUrl = "https://example.localhost/second-test-card",
            Summary = "Summary text for the second test card.",
        };

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                SupportCards = new CardContent[] {
                    firstTestCardContent,
                    secondTestCardContent,
                },
            });

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual(2, model.SupportCards.Count);
        Assert.AreSame(firstTestCardContent, model.SupportCards[0]);
        Assert.AreSame(secondTestCardContent, model.SupportCards[1]);
    }

    #endregion
}
