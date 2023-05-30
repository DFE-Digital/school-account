namespace Dfe.SchoolAccount.Web.Tests.Services.ContentHyperlinks.Handlers;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;

[TestClass]
public sealed class SignpostingSignpostingPageContentHyperlinkResolutionHandlerTests
{
    #region ContentHyperlink? ResolveContentHyperlink(SignpostingPageContent)

    [TestMethod]
    public void ResolveContentHyperlink__ReturnsExpectedContentHyperlink()
    {
        var fakeContent = new SignpostingPageContent {
            Title = "An example signposting page",
            Slug = "example-slug"
        };

        var signpostingPageContentHyperlinkResolutionHandler = new SignpostingPageContentHyperlinkResolutionHandler();

        var contentHyperlink = signpostingPageContentHyperlinkResolutionHandler.ResolveContentHyperlink(fakeContent)!;

        Assert.AreEqual("An example signposting page", contentHyperlink.Text);
        Assert.AreEqual("/signposting/example-slug", contentHyperlink.Url);
    }

    #endregion
}
