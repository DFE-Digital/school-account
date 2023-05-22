namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;

[TestClass]
public sealed class ExternalResourceContentToCardTransformHandlerTests
{
    #region CardViewModel TransformContentToViewModel(ExternalResourceContent)

    [TestMethod]
    public void TransformContentToViewModel__ReturnsCardViewModelWithExpectedProperties()
    {
        var externalResourceContent = new ExternalResourceContent {
            Title = "External resource title",
            Summary = "Summary of the external resource.",
            LinkUrl = "https://example.localhost:12345",
        };

        var externalResourceContentToCardTransformHandler = new ExternalResourceContentToCardTransformHandler();

        var cardModel = externalResourceContentToCardTransformHandler.TransformContentToModel(externalResourceContent);

        Assert.AreEqual("External resource title", cardModel.Heading);
        Assert.AreEqual("Summary of the external resource.", cardModel.Summary);
        Assert.AreEqual("https://example.localhost:12345", cardModel.LinkUrl);
    }

    #endregion
}
