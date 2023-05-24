namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers.Cards;

using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Moq;

[TestClass]
public sealed class ContentModelTransformerExtensionsTests
{
    #region TTargetModel[] TransformContentToModel<TTargetModel>(IContentModelTransformer, IEnumerable<IContent>)

    [TestMethod]
    public void TransformContentToModel_TTargetModel__ThrowsArgumentNullException__WhenTransformerArgumentIsNull()
    {
        var fakeCardContent = Array.Empty<IContent>();

        var act = () => {
            _ = ContentModelTransformerExtensions.TransformContentToModel<CardModel>(null!, fakeCardContent);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void TransformContentToModel_TTargetModel__ThrowsArgumentNullException__WhenContentArgumentIsNull()
    {
        var transformer = new Mock<IContentModelTransformer>();
        var fakeCardContent = Array.Empty<IContent>();

        var act = () => {
            _ = ContentModelTransformerExtensions.TransformContentToModel<CardModel>(transformer.Object, null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void TransformContentToModel_TTargetModel__InvokesTransformerForEachContentEntry()
    {
        var transformer = new Mock<IContentModelTransformer>();
        transformer.Setup(mock => mock.TransformContentToModel<CardModel>(It.IsAny<ExternalResourceContent>()))
            .Returns<ExternalResourceContent>(content => new CardModel {
                Heading = content.Title,
            });

        var fakeCardContent = new IContent[] {
            new ExternalResourceContent { Title = "Test1" },
            new ExternalResourceContent { Title = "Test2" },
            new ExternalResourceContent { Title = "Test3" },
        };

        var resultingModels = ContentModelTransformerExtensions.TransformContentToModel<CardModel>(transformer.Object, fakeCardContent);

        Assert.AreEqual(3, resultingModels.Length);
        Assert.AreEqual("Test1", resultingModels[0].Heading);
        Assert.AreEqual("Test2", resultingModels[1].Heading);
        Assert.AreEqual("Test3", resultingModels[2].Heading);
    }

    #endregion
}
