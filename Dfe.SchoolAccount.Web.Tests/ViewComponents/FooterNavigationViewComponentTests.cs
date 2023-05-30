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
        var fakeContentCollection = new[] {
            new ExternalResourceContent(),
            new ExternalResourceContent(),
            new ExternalResourceContent(),
        };
        var fakeContentHyperlinks = new[] {
            new ContentHyperlink(),
            new ContentHyperlink(),
        };

        var websiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();
        websiteGlobalsContentFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsContentAsync())
            .ReturnsAsync(new WebsiteGlobalsContent {
                FooterLinks = fakeContentCollection, 
            });

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        contentHyperlinkResolverMock.SetupSequence(mock => mock.ResolveContentHyperlink(It.IsIn(fakeContentCollection)))
            .Returns(fakeContentHyperlinks[0])
            .Returns((ContentHyperlink?)null)
            .Returns(fakeContentHyperlinks[1]);

        var footerNavigationViewComponent = new FooterNavigationViewComponent(
            websiteGlobalsContentFetcherMock.Object,
            contentHyperlinkResolverMock.Object
        );

        var viewComponentResult = await footerNavigationViewComponent.InvokeAsync();
        var viewViewComponentResult = TypeAssert.IsType<ViewViewComponentResult>(viewComponentResult);
        var viewModel = TypeAssert.IsType<FooterNavigationViewModel>(viewViewComponentResult.ViewData!.Model);
        CollectionAssert.AreEqual(fakeContentHyperlinks, viewModel.Links.ToArray());
    }

    #endregion
}
