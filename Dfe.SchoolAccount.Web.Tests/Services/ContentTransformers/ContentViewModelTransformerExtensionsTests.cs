namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers.Cards;

using Contentful.Core.Models;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Moq;

[TestClass]
public sealed class ContentViewModelTransformerExtensionsTests
{
    #region CardViewModel[] TransformContentToViewModel<TViewModel>(IContentViewModelTransformer, IEnumerable<IContent>)

    [TestMethod]
    public void TransformContentToViewModel_TViewModel__ThrowsArgumentNullException__WhenTransformerArgumentIsNull()
    {
        var fakeCardContent = Array.Empty<IContent>();

        var act = () => {
            _ = ContentViewModelTransformerExtensions.TransformContentToViewModel<CardViewModel>(null!, fakeCardContent);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void TransformContentToViewModel_TViewModel__ThrowsArgumentNullException__WhenContentArgumentIsNull()
    {
        var transformer = new Mock<IContentViewModelTransformer>();
        var fakeCardContent = Array.Empty<IContent>();

        var act = () => {
            _ = ContentViewModelTransformerExtensions.TransformContentToViewModel<CardViewModel>(transformer.Object, null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void TransformContentToViewModel_TViewModel__InvokesTransformerForEachContentEntry()
    {
        var transformer = new Mock<IContentViewModelTransformer>();
        transformer.Setup(mock => mock.TransformContentToViewModel<CardViewModel>(It.IsAny<ExternalResourceContent>()))
            .Returns<ExternalResourceContent>(content => new CardViewModel {
                Heading = content.Title,
            });

        var fakeCardContent = new IContent[] {
            new ExternalResourceContent { Title = "Test1" },
            new ExternalResourceContent { Title = "Test2" },
            new ExternalResourceContent { Title = "Test3" },
        };

        var viewModels = ContentViewModelTransformerExtensions.TransformContentToViewModel<CardViewModel>(transformer.Object, fakeCardContent);

        Assert.AreEqual(3, viewModels.Length);
        Assert.AreEqual("Test1", viewModels[0].Heading);
        Assert.AreEqual("Test2", viewModels[1].Heading);
        Assert.AreEqual("Test3", viewModels[2].Heading);
    }

    #endregion
}
