namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Moq;

[TestClass]
public sealed class ContentfulErrorPageContentFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var act = () => {
            _ = new ContentfulErrorPageContentFetcher(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<ErrorPageContent> FetchErrorPageContentAsync(string)

    [TestMethod]
    public async Task FetchErrorPageContentAsync__ThrowsArgumentNullException__WhenHandleArgumentIsNull()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentfulErrorPageContentFetcher = new ContentfulErrorPageContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulErrorPageContentFetcher.FetchErrorPageContentAsync(null!);
        };

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(act);
    }

    [TestMethod]
    public async Task FetchErrorPageContentAsync__ThrowsArgumentException__WhenHandleArgumentIsAnEmptyString()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentfulErrorPageContentFetcher = new ContentfulErrorPageContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulErrorPageContentFetcher.FetchErrorPageContentAsync("");
        };

        var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(act);
        Assert.AreEqual("Cannot be an empty string. (Parameter 'handle')", exception.Message);
    }

    [TestMethod]
    public async Task FetchErrorPageContentAsync__ReturnsNull__WhenContentfulClientReturnsNoEntries()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<ErrorPageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<ErrorPageContent> {
                Items = Array.Empty<ErrorPageContent>(),
            });

        var contentfulErrorPageContentFetcher = new ContentfulErrorPageContentFetcher(contentfulClientMock.Object);

        var entry = await contentfulErrorPageContentFetcher.FetchErrorPageContentAsync("a-handle-with-does-not-exist");

        Assert.IsNull(entry);
    }

    [TestMethod]
    public async Task FetchErrorPageContentAsync__ReturnsResultFromContentfulClient()
    {
        var fakeErrorPageContent = new ErrorPageContent();

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<ErrorPageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<ErrorPageContent> {
                Items = new ErrorPageContent[] { fakeErrorPageContent },
            });

        var contentfulErrorPageContentFetcher = new ContentfulErrorPageContentFetcher(contentfulClientMock.Object);

        var entry = await contentfulErrorPageContentFetcher.FetchErrorPageContentAsync("an-example-handle");

        Assert.AreSame(fakeErrorPageContent, entry);
    }

    #endregion
}
