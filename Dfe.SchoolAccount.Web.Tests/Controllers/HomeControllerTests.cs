namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
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
    private static HomeController CreateHomeControllerWithCommunitySchoolUser(IHubContentFetcher hubContentFetcher, IContentViewModelTransformer contentViewModelTransformer)
    {
        var logger = new NullLogger<HomeController>();

        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();
        var personaResolver = new OrganisationTypePersonaResolver();

        return new HomeController(logger, personaResolver, hubContentFetcher, contentViewModelTransformer) {
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

        var contentViewModelTransformerMock = new Mock<IContentViewModelTransformer>();

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentViewModelTransformerMock.Object);

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

        var contentViewModelTransformerMock = new Mock<IContentViewModelTransformer>();

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentViewModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual("An example organisation name", model.OrganisationName);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithUsefulServicesAndGuidanceCards()
    {
        var cardContent = new ExternalResourceContent();
        var cardViewModel = new CardViewModel();

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                UsefulServicesAndGuidanceCards = new IContent[] { cardContent },
            });

        var contentViewModelTransformerMock = new Mock<IContentViewModelTransformer>();
        contentViewModelTransformerMock.Setup(mock => mock.TransformContentToViewModel<CardViewModel>(cardContent))
            .Returns(cardViewModel);

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentViewModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        CollectionAssert.AreEqual(new CardViewModel[] { cardViewModel }, model.UsefulServicesAndGuidanceCards.ToArray());
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithSupportCards()
    {
        var cardContent = new SignpostingPageContent();
        var cardViewModel = new CardViewModel();

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                SupportCards = new IContent[] { cardContent },
            });

        var contentViewModelTransformerMock = new Mock<IContentViewModelTransformer>();
        contentViewModelTransformerMock.Setup(mock => mock.TransformContentToViewModel<CardViewModel>(It.Is<IContent>(content => content == cardContent)))
            .Returns(cardViewModel);

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentViewModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        CollectionAssert.AreEqual(new CardViewModel[] { cardViewModel }, model.SupportCards.ToArray());
    }

    #endregion
}
