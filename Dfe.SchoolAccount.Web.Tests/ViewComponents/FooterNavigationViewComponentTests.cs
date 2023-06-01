namespace Dfe.SchoolAccount.Web.Tests.ViewComponents;

using Dfe.SchoolAccount.Web.Models.Components;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Dfe.SchoolAccount.Web.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;

[TestClass]
public sealed class FooterNavigationViewComponentTests
{
    #region Task<IViewComponentResult> InvokeAsync()

    [TestMethod]
    public async Task InvokeAsync__ReturnsViewWithExpectedLinks()
    {
        var fakeContentHyperlinks = new[] {
            new ContentHyperlink(),
            new ContentHyperlink(),
        };

        var websiteGlobalsFetcherMock = new Mock<IWebsiteGlobalsFetcher>();
        websiteGlobalsFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsAsync())
            .ReturnsAsync(new WebsiteGlobalsModel {
                FooterLinks = fakeContentHyperlinks, 
            });

        var footerNavigationViewComponent = new FooterNavigationViewComponent(
            websiteGlobalsFetcherMock.Object
        );

        var viewComponentResult = await footerNavigationViewComponent.InvokeAsync();
        var viewViewComponentResult = TypeAssert.IsType<ViewViewComponentResult>(viewComponentResult);
        var viewModel = TypeAssert.IsType<FooterNavigationViewModel>(viewViewComponentResult.ViewData!.Model);
        CollectionAssert.AreEqual(fakeContentHyperlinks, viewModel.Links.ToArray());
    }

    #endregion
}
