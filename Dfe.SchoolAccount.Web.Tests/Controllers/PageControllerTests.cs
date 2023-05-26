namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class PageControllerTests
{
    private static PageController CreatePageControllerWithFakeContent(PageContent fakePage)
    {
        var logger = new NullLogger<PageController>();

        var pageContentFetcherMock = new Mock<IPageContentFetcher>();
        pageContentFetcherMock.Setup(mock => mock.FetchPageContentAsync(It.Is<string>(slug => slug == "an-example-page-handle")))
            .ReturnsAsync(fakePage);

        return new PageController(logger, pageContentFetcherMock.Object);
    }

    #region Task<IActionResult> Index(string)

    [TestMethod]
    public async Task Index__ReturnsNotFound__WhenContentForHandleWasNotFound()
    {
        var fakePage = new PageContent();
        var pageController = CreatePageControllerWithFakeContent(fakePage);

        var result = await pageController.Index("a-slug-that-does-not-exist");

        TypeAssert.IsType<NotFoundResult>(result);
    }

    [TestMethod]
    public async Task Index__ReturnsExpectedView()
    {
        var fakePage = new PageContent();
        var pageController = CreatePageControllerWithFakeContent(fakePage);

        var result = await pageController.Index("an-example-page-handle");

        TypeAssert.IsType<ViewResult>(result);
    }

    [TestMethod]
    public async Task Index__ProvidesPageContentAsViewModel()
    {
        var fakePage = new PageContent();
        var pageController = CreatePageControllerWithFakeContent(fakePage);

        var result = await pageController.Index("an-example-page-handle");

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreSame(fakePage, viewResult.Model);
    }

    #endregion
}
