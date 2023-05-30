namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;
using Moq;

[TestClass]
public sealed class SignpostingPageContentToCardTransformHandlerTests
{
    #region CardViewModel TransformContentToViewModel(SignpostingPageContent)

    [TestMethod]
    public void TransformContentToViewModel__ReturnsCardViewModelWithExpectedProperties()
    {
        var signpostingPageContent = new SignpostingPageContent {
            Slug = "example-signposting-page",
            Title = "Example signposting page",
            Summary = "Summary of the signposting page.",
        };

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        contentHyperlinkResolverMock.Setup(mock => mock.ResolveContentHyperlink(It.Is<SignpostingPageContent>(param => param == signpostingPageContent)))
            .Returns(new ContentHyperlink { Url = "/signposting/example-signposting-page" });

        var signpostingPageContentToCardTransformHandler = new SignpostingPageContentToCardTransformHandler(contentHyperlinkResolverMock.Object);

        var cardModel = signpostingPageContentToCardTransformHandler.TransformContentToModel(signpostingPageContent);

        Assert.AreEqual("Example signposting page", cardModel.Heading);
        Assert.AreEqual("Summary of the signposting page.", cardModel.Summary);
        Assert.AreEqual("/signposting/example-signposting-page", cardModel.LinkUrl);
    }

    #endregion
}
