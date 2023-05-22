namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class SignpostingPageControllerTests
{
    private static SignpostingController CreateSignpostingControllerWithFakeContent()
    {
        var logger = new NullLogger<SignpostingController>();

        var fakeSignpostingPage = new SignpostingPageContent {
            Title = "An example signposting page",
            Body = new Document {
                Content = new List<IContent> {
                    new Paragraph(),
                },
            },
        };

        var signpostingPageContentFetcherMock = new Mock<ISignpostingPageContentFetcher>();
        signpostingPageContentFetcherMock.Setup(mock => mock.FetchSignpostingPageContentAsync(It.Is<string>(slug => slug == "an-example-slug")))
            .ReturnsAsync(fakeSignpostingPage);

        return new SignpostingController(logger, signpostingPageContentFetcherMock.Object);
    }

    #region Task<IActionResult> Index(string)

    [TestMethod]
    public async Task Index__ReturnsNotFound__WhenContentForSlugWasNotFound()
    {
        var logger = new NullLogger<SignpostingController>();
        var signpostingPageContentFetcherMock = new Mock<ISignpostingPageContentFetcher>();
        var signpostingPageController = new SignpostingController(logger, signpostingPageContentFetcherMock.Object);

        var result = await signpostingPageController.Index("a-slug-that-does-not-exist");

        TypeAssert.IsType<NotFoundResult>(result);
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
