namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class ErrorPageControllerTests
{
    private static ErrorPageController CreateErrorPageControllerWithFakeContent(ErrorPageContent fakeErrorPage)
    {
        var logger = new NullLogger<ErrorPageController>();

        var errorPageContentFetcherMock = new Mock<IErrorPageContentFetcher>();
        errorPageContentFetcherMock.Setup(mock => mock.FetchErrorPageContentAsync(It.Is<string>(slug => slug == "an-example-error-handle")))
            .ReturnsAsync(fakeErrorPage);

        return new ErrorPageController(logger, errorPageContentFetcherMock.Object);
    }

    #region Task<IActionResult> Index(string)

    [TestMethod]
    public async Task Index__ReturnsNotFound__WhenContentForHandleWasNotFound()
    {
        var fakeErrorPage = new ErrorPageContent();
        var errorPageController = CreateErrorPageControllerWithFakeContent(fakeErrorPage);

        var result = await errorPageController.Index("a-slug-that-does-not-exist", 403);

        TypeAssert.IsType<NotFoundResult>(result);
    }

    [TestMethod]
    public async Task Index__ReturnsExpectedView()
    {
        var fakeErrorPage = new ErrorPageContent();
        var errorPageController = CreateErrorPageControllerWithFakeContent(fakeErrorPage);

        var result = await errorPageController.Index("an-example-error-handle", 403);

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreEqual(403, viewResult.StatusCode);
    }

    [TestMethod]
    public async Task Index__ProvidesErrorPageContentAsViewModel()
    {
        var fakeErrorPage = new ErrorPageContent();
        var errorPageController = CreateErrorPageControllerWithFakeContent(fakeErrorPage);

        var result = await errorPageController.Index("an-example-error-handle", 403);

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreSame(fakeErrorPage, viewResult.Model);
    }

    #endregion
}
