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
    private static HomeController CreateHomeControllerWithCommunitySchoolUser(IHubContentFetcher hubContentFetcher, IContentModelTransformer contentModelTransformer)
    {
        var logger = new NullLogger<HomeController>();

        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();
        var personaResolver = new OrganisationTypePersonaResolver();

        return new HomeController(logger, personaResolver, hubContentFetcher, contentModelTransformer) {
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

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentModelTransformerMock.Object);

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

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual("An example organisation name", model.OrganisationName);
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithUsefulServicesAndGuidanceCards()
    {
        var cardContent = new ExternalResourceContent();
        var cardViewModel = new CardModel();

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                UsefulServicesAndGuidanceCards = new IContent[] { cardContent },
            });

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        contentModelTransformerMock.Setup(mock => mock.TransformContentToModel<CardModel>(cardContent))
            .Returns(cardViewModel);

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        CollectionAssert.AreEqual(new CardModel[] { cardViewModel }, model.UsefulServicesAndGuidanceCards.ToArray());
    }

    [TestMethod]
    public async Task Index__PopulatesViewModelWithSupportCards()
    {
        var cardContent = new SignpostingPageContent();
        var cardViewModel = new CardModel();

        var hubContentFetcherMock = new Mock<IHubContentFetcher>();
        hubContentFetcherMock.Setup(mock => mock.FetchHubContentAsync(It.IsAny<PersonaName>()))
            .ReturnsAsync(new HubContent {
                SupportCards = new IContent[] { cardContent },
            });

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        contentModelTransformerMock.Setup(mock => mock.TransformContentToModel<CardModel>(It.Is<IContent>(content => content == cardContent)))
            .Returns(cardViewModel);

        var homeController = CreateHomeControllerWithCommunitySchoolUser(hubContentFetcherMock.Object, contentModelTransformerMock.Object);

        var result = await homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        CollectionAssert.AreEqual(new CardModel[] { cardViewModel }, model.SupportCards.ToArray());
    }

    #endregion
}
