namespace Dfe.SchoolAccount.Web.Tests.Services.ContentHyperlinks;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Moq;

[TestClass]
public sealed class RegisteredServicesContentHyperlinkResolverTests
{
    #region Constructor(IServiceProvider)

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenServiceProviderArgumentIsNull()
    {
        var act = () => {
            _ = new RegisteredServicesContentHyperlinkResolver(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    #endregion

    #region ContentHyperlink? ResolveContentHyperlink(object)

    [TestMethod]
    public void ResolveContentHyperlink__ThrowsArgumentNullException__WhenContentArgumentIsNull()
    {
        var serviceProvider = new Mock<IServiceProvider>();

        var registeredServicesContentHyperlinkResolver = new RegisteredServicesContentHyperlinkResolver(serviceProvider.Object);

        var act = () => {
            _ = registeredServicesContentHyperlinkResolver.ResolveContentHyperlink(null!);
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("content", exception.ParamName);
    }

    [TestMethod]
    public void ResolveContentHyperlink__ReturnsNull__WhenHandlerIsNotRegistered()
    {
        var serviceProvider = new Mock<IServiceProvider>();

        var registeredServicesContentHyperlinkResolver = new RegisteredServicesContentHyperlinkResolver(serviceProvider.Object);
        var fakeContent = new ExternalResourceContent();

        var contentHyperlink = registeredServicesContentHyperlinkResolver.ResolveContentHyperlink(fakeContent);

        Assert.IsNull(contentHyperlink);
    }

    [TestMethod]
    public void ResolveContentHyperlink__InvokesExpectedHandler()
    {
        var mockHandler = new Mock<IContentHyperlinkResolutionHandler<ExternalResourceContent>>();

        var serviceProvider = new Mock<IServiceProvider>();
        var expectedServiceType = typeof(IContentHyperlinkResolutionHandler<ExternalResourceContent>);
        serviceProvider.Setup(mock => mock.GetService(It.Is<Type>(serviceType => serviceType == expectedServiceType)))
            .Returns(mockHandler.Object);

        var registeredServicesContentHyperlinkResolver = new RegisteredServicesContentHyperlinkResolver(serviceProvider.Object);
        var fakeContent = new ExternalResourceContent();

        _ = registeredServicesContentHyperlinkResolver.ResolveContentHyperlink(fakeContent);

        mockHandler.Verify(mock => mock.ResolveContentHyperlink(It.Is<ExternalResourceContent>(content => content == fakeContent)));
    }

    #endregion
}
