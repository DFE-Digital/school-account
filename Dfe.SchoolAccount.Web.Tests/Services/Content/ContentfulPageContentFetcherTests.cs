namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Moq;

[TestClass]
public sealed class ContentfulPageContentFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var act = () => {
            _ = new ContentfulPageContentFetcher(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<PageContent> FetchPageContentAsync(string)

    [TestMethod]
    public async Task FetchPageContentAsync__ThrowsArgumentNullException__WhenHandleArgumentIsNull()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentfulPageContentFetcher = new ContentfulPageContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulPageContentFetcher.FetchPageContentAsync(null!);
        };

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(act);
    }

    [TestMethod]
    public async Task FetchPageContentAsync__ThrowsArgumentException__WhenHandleArgumentIsAnEmptyString()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentfulPageContentFetcher = new ContentfulPageContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulPageContentFetcher.FetchPageContentAsync("");
        };

        var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(act);
        Assert.AreEqual("Cannot be an empty string. (Parameter 'slug')", exception.Message);
    }

    [TestMethod]
    public async Task FetchPageContentAsync__ReturnsNull__WhenContentfulClientReturnsNoEntries()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<PageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<PageContent> {
                Items = Array.Empty<PageContent>(),
            });

        var contentfulPageContentFetcher = new ContentfulPageContentFetcher(contentfulClientMock.Object);

        var entry = await contentfulPageContentFetcher.FetchPageContentAsync("a-handle-with-does-not-exist");

        Assert.IsNull(entry);
    }

    [TestMethod]
    public async Task FetchPageContentAsync__ReturnsResultFromContentfulClient()
    {
        var fakePageContent = new PageContent();

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<PageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<PageContent> {
                Items = new PageContent[] { fakePageContent },
            });

        var contentfulPageContentFetcher = new ContentfulPageContentFetcher(contentfulClientMock.Object);

        var entry = await contentfulPageContentFetcher.FetchPageContentAsync("an-example-handle");

        Assert.AreSame(fakePageContent, entry);
    }

    #endregion
}
