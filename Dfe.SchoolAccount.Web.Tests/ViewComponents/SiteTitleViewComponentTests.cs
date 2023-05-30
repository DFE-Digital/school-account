namespace Dfe.SchoolAccount.Web.Tests.ViewComponents;

using Dfe.SchoolAccount.Web.Models.Components;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Dfe.SchoolAccount.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;

[TestClass]
public sealed class SiteTitleViewComponentTests
{
    #region Task<IViewComponentResult> InvokeAsync()

    [TestMethod]
    public async Task InvokeAsync__ReturnsViewWithExpectedSiteTitle()
    {
        var websiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();
        websiteGlobalsContentFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsContentAsync())
            .ReturnsAsync(new WebsiteGlobalsContent {
                SiteTitle = "Example site title",
            });

        var siteTitleViewComponent = new SiteTitleViewComponent(
            websiteGlobalsContentFetcherMock.Object
        );

        var viewComponentResult = await siteTitleViewComponent.InvokeAsync();
        var viewViewComponentResult = TypeAssert.IsType<ViewViewComponentResult>(viewComponentResult);
        var viewModel = TypeAssert.IsType<SiteTitleViewModel>(viewViewComponentResult.ViewData!.Model);
        Assert.AreEqual("Example site title", viewModel.SiteTitle);
    }

    #endregion
}
