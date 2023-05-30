namespace Dfe.SchoolAccount.Web.Tests.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

[TestClass]
public sealed class PageContentHyperlinkResolutionHandlerTests
{
    #region ContentHyperlink? ResolveContentHyperlink(PageContent)

    [TestMethod]
    public void ResolveContentHyperlink__ReturnsExpectedContentHyperlink()
    {
        var fakeContent = new PageContent {
            Title = "An example page",
            Slug = "example-slug"
        };

        var pageContentHyperlinkResolutionHandler = new PageContentHyperlinkResolutionHandler();

        var contentHyperlink = pageContentHyperlinkResolutionHandler.ResolveContentHyperlink(fakeContent)!;

        Assert.AreEqual("An example page", contentHyperlink.Text);
        Assert.AreEqual("/example-slug", contentHyperlink.Url);
    }

    #endregion
}
