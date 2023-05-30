namespace Dfe.SchoolAccount.Web.Tests.Services.ContentTransformers;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Moq;

[TestClass]
public sealed class RegisteredServicesContentModelTransformerTests
{
    #region Constructor(IServiceProvider)

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenServiceProviderArgumentIsNull()
    {
        var act = () => {
            _ = new RegisteredServicesContentModelTransformer(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    #endregion

    #region TTargetModel TransformContentToModel<TTargetModel>(ExternalResourceContent)

    [TestMethod]
    public void TransformContentToModel_TTargetModel__ThrowsInvalidOperationException__WhenTransformHandlerIsNotRegistered()
    {
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()));

        var registeredServicesContentModelTransformer = new RegisteredServicesContentModelTransformer(serviceProvider.Object);
        var fakeCardContent = new ExternalResourceContent();

        var act = () => {
            _ = registeredServicesContentModelTransformer.TransformContentToModel<CardModel>(fakeCardContent);
        };

        var exception = Assert.ThrowsException<InvalidOperationException>(act);
        Assert.AreEqual($"Cannot resolve transform handler for `{typeof(ExternalResourceContent)}` to `{typeof(CardModel)}`.", exception.Message);
    }

    [TestMethod]
    public void TransformContentToModel_TTargetModel__InvokesExpectedTransformHandler()
    {
        var mockTransformHandler = new Mock<IContentModelTransformHandler<ExternalResourceContent, CardModel>>();

        var serviceProvider = new Mock<IServiceProvider>();
        var expectedServiceType = typeof(IContentModelTransformHandler<ExternalResourceContent, CardModel>);
        serviceProvider.Setup(mock => mock.GetService(It.Is<Type>(serviceType => serviceType == expectedServiceType)))
            .Returns(mockTransformHandler.Object);

        var registeredServicesContentModelTransformer = new RegisteredServicesContentModelTransformer(serviceProvider.Object);
        var fakeCardContent = new ExternalResourceContent();

        _ = registeredServicesContentModelTransformer.TransformContentToModel<CardModel>(fakeCardContent);

        mockTransformHandler.Verify(mock => mock.TransformContentToModel(It.Is<ExternalResourceContent>(content => content == fakeCardContent)));
    }

    #endregion
}
