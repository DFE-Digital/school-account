namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Moq;

[TestClass]
public sealed class ContentfulWebsiteGlobalsContentFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var act = () => {
            _ = new ContentfulWebsiteGlobalsContentFetcher(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<WebsiteGlobalsContent> FetchWebsiteGlobalsContentAsync()

    [TestMethod]
    public async Task FetchWebsiteGlobalsContentAsync__ThrowsInvalidOperationException__WhenWebsiteGlobalsEntryWasNotFound()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = Array.Empty<WebsiteGlobalsContent>(),
            });

        var contentfulWebsiteGlobalsContentFetcher = new ContentfulWebsiteGlobalsContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulWebsiteGlobalsContentFetcher.FetchWebsiteGlobalsContentAsync();
        };

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsContentAsync__ThrowsInvalidOperationException__WhenMultipleWebsiteGlobalsEntriesWereFound()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = new WebsiteGlobalsContent[] {
                    new WebsiteGlobalsContent(),
                    new WebsiteGlobalsContent(),
                },
            });

        var contentfulWebsiteGlobalsContentFetcher = new ContentfulWebsiteGlobalsContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulWebsiteGlobalsContentFetcher.FetchWebsiteGlobalsContentAsync();
        };

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsContentAsync__ReturnsResultFromContentfulClient()
    {
        var fakeWebsiteGlobalsContent = new WebsiteGlobalsContent();

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = new WebsiteGlobalsContent[] {
                    fakeWebsiteGlobalsContent,
                },
            });

        var contentfulWebsiteGlobalsContentFetcher = new ContentfulWebsiteGlobalsContentFetcher(contentfulClientMock.Object);

        var entry = await contentfulWebsiteGlobalsContentFetcher.FetchWebsiteGlobalsContentAsync();

        Assert.AreSame(fakeWebsiteGlobalsContent, entry);
    }

    #endregion
}
