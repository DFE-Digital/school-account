namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers.Cards;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Moq;

[TestClass]
public sealed class RegisteredServicesContentViewModelTransformerTests
{
    #region Constructor(IServiceProvider)

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenServiceProviderArgumentIsNull()
    {
        var act = () => {
            _ = new RegisteredServicesContentViewModelTransformer(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    #endregion

    #region CardViewModel TransformContentToViewModel<TViewModel>(ExternalResourceContent)

    [TestMethod]
    public void TransformContentToViewModel_TViewModel__ThrowsInvalidOperationException__WhenTransformHandlerIsNotRegistered()
    {
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()));

        var registeredServicesContentViewModelTransformer = new RegisteredServicesContentViewModelTransformer(serviceProvider.Object);
        var fakeCardContent = new ExternalResourceContent();

        var act = () => {
            _ = registeredServicesContentViewModelTransformer.TransformContentToViewModel<CardModel>(fakeCardContent);
        };

        var exception = Assert.ThrowsException<InvalidOperationException>(act);
        Assert.AreEqual($"Cannot resolve transform handler for `{typeof(ExternalResourceContent)}` to `{typeof(CardModel)}`.", exception.Message);
    }

    [TestMethod]
    public void TransformContentToViewModel_TViewModel__InvokesExpectedTransformHandler()
    {
        var mockTransformHandler = new Mock<IContentViewModelTransformHandler<ExternalResourceContent, CardModel>>();

        var serviceProvider = new Mock<IServiceProvider>();
        var expectedServiceType = typeof(IContentViewModelTransformHandler<ExternalResourceContent, CardModel>);
        serviceProvider.Setup(mock => mock.GetService(It.Is<Type>(serviceType => serviceType == expectedServiceType)))
            .Returns(mockTransformHandler.Object);

        var registeredServicesContentViewModelTransformer = new RegisteredServicesContentViewModelTransformer(serviceProvider.Object);
        var fakeCardContent = new ExternalResourceContent();

        _ = registeredServicesContentViewModelTransformer.TransformContentToViewModel<CardModel>(fakeCardContent);

        mockTransformHandler.Verify(mock => mock.TransformContentToViewModel(It.Is<ExternalResourceContent>(content => content == fakeCardContent)));
    }

    #endregion
}
