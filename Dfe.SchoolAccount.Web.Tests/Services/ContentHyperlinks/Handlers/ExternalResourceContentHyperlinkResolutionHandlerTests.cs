namespace Dfe.SchoolAccount.Web.Tests.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

[TestClass]
public sealed class ExternalResourceContentHyperlinkResolutionHandlerTests
{
    #region ContentHyperlink? ResolveContentHyperlink(ExternalResourceContent)

    [TestMethod]
    public void ResolveContentHyperlink__ReturnsExpectedContentHyperlink()
    {
        var fakeContent = new ExternalResourceContent {
            Title = "Test external resource",
            LinkUrl = "https://example.localhost/test-external-resource",
        };

        var externalResourceContentHyperlinkResolutionHandler = new ExternalResourceContentHyperlinkResolutionHandler();

        var contentHyperlink = externalResourceContentHyperlinkResolutionHandler.ResolveContentHyperlink(fakeContent)!;

        Assert.AreEqual("Test external resource", contentHyperlink.Text);
        Assert.AreEqual("https://example.localhost/test-external-resource", contentHyperlink.Url);
    }

    #endregion
}
