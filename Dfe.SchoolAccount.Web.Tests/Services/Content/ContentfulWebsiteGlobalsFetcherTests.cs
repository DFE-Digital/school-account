namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Moq;

[TestClass]
public sealed class ContentfulWebsiteGlobalsFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();

        var act = () => {
            _ = new ContentfulWebsiteGlobalsFetcher(null!, contentHyperlinkResolverMock.Object);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentHyperlinkResolverArgumentIsNull()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();

        var act = () => {
            _ = new ContentfulWebsiteGlobalsFetcher(contentfulClientMock.Object, null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<WebsiteGlobalsModel> FetchWebsiteGlobalsAsync()

    [TestMethod]
    public async Task FetchWebsiteGlobalsAsync__ThrowsInvalidOperationException__WhenWebsiteGlobalsEntryWasNotFound()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = Array.Empty<WebsiteGlobalsContent>(),
            });

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        var contentfulWebsiteGlobalsFetcher = new ContentfulWebsiteGlobalsFetcher(contentfulClientMock.Object, contentHyperlinkResolverMock.Object);

        var act = async () => {
            _ = await contentfulWebsiteGlobalsFetcher.FetchWebsiteGlobalsAsync();
        };

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsAsync__ThrowsInvalidOperationException__WhenMultipleWebsiteGlobalsEntriesWereFound()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = new WebsiteGlobalsContent[] {
                    new WebsiteGlobalsContent(),
                    new WebsiteGlobalsContent(),
                },
            });

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        var contentfulWebsiteGlobalsFetcher = new ContentfulWebsiteGlobalsFetcher(contentfulClientMock.Object, contentHyperlinkResolverMock.Object);

        var act = async () => {
            _ = await contentfulWebsiteGlobalsFetcher.FetchWebsiteGlobalsAsync();
        };

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsAsync__ReturnsExpectedResult()
    {
        var fakeWebsiteGlobalsContent = new WebsiteGlobalsContent {
            SiteTitle = "Example site title",
            FooterLinks = new IContent[] {
                new ExternalResourceContent {
                    Title = "First link",
                    LinkUrl = "https://example.localhost/first",
                },
                new ExternalResourceContent {
                    Title = "Second link",
                    LinkUrl = "https://example.localhost/second",
                },
            },
        };

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<WebsiteGlobalsContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<WebsiteGlobalsContent> {
                Items = new WebsiteGlobalsContent[] { fakeWebsiteGlobalsContent },
            });

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        contentHyperlinkResolverMock.Setup(mock => mock.ResolveContentHyperlink(It.IsAny<object>()))
            .Returns((object content) => new ContentHyperlink {
                Text = ((ExternalResourceContent)content).Title,
                Url = ((ExternalResourceContent)content).LinkUrl,
            });

        var contentfulWebsiteGlobalsFetcher = new ContentfulWebsiteGlobalsFetcher(contentfulClientMock.Object, contentHyperlinkResolverMock.Object);

        var websiteGlobals = await contentfulWebsiteGlobalsFetcher.FetchWebsiteGlobalsAsync();

        Assert.AreEqual("Example site title", websiteGlobals.SiteTitle);
        Assert.AreEqual("First link", websiteGlobals.FooterLinks[0].Text);
        Assert.AreEqual("https://example.localhost/first", websiteGlobals.FooterLinks[0].Url);
        Assert.AreEqual("Second link", websiteGlobals.FooterLinks[1].Text);
        Assert.AreEqual("https://example.localhost/second", websiteGlobals.FooterLinks[1].Url);
    }

    #endregion
}
