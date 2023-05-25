namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Moq;

[TestClass]
public sealed class ContentfulSignpostingPageContentFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var contentModelTransformerMock = new Mock<IContentModelTransformer>();

        var act = () => {
            _ = new ContentfulSignpostingPageContentFetcher(null!, contentModelTransformerMock.Object);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentModelTransformerArgumentIsNull()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();

        var act = () => {
            _ = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<SignpostingPageContent> FetchSignpostingPageContentAsync(string)

    [TestMethod]
    public async Task FetchSignpostingPageContentAsync__ThrowsArgumentNullException__WhenSlugArgumentIsNull()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        var contentfulHubContentFetcher = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, contentModelTransformerMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchSignpostingPageContentAsync(null!);
        };

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(act);
    }

    [TestMethod]
    public async Task FetchSignpostingPageContentAsync__ThrowsArgumentException__WhenSlugArgumentIsAnEmptyString()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        var contentfulHubContentFetcher = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, contentModelTransformerMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchSignpostingPageContentAsync("");
        };

        var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(act);
        Assert.AreEqual("Cannot be an empty string. (Parameter 'slug')", exception.Message);
    }

    [TestMethod]
    public async Task FetchSignpostingPageContentAsync__ReturnsNull__WhenContentfulClientReturnsNoEntries()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<SignpostingPageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<SignpostingPageContent> {
                Items = Array.Empty<SignpostingPageContent>(),
            });

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        var contentfulHubContentFetcher = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, contentModelTransformerMock.Object);

        var entry = await contentfulHubContentFetcher.FetchSignpostingPageContentAsync("a-slug-with-does-not-exist");

        Assert.IsNull(entry);
    }

    [TestMethod]
    public async Task FetchSignpostingPageContentAsync__ReturnsResultFromContentfulClient()
    {
        var fakeSignpostingPageContent = new SignpostingPageContent();

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<SignpostingPageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<SignpostingPageContent> {
                Items = new SignpostingPageContent[] { fakeSignpostingPageContent },
            });

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        var contentfulHubContentFetcher = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, contentModelTransformerMock.Object);

        var entry = await contentfulHubContentFetcher.FetchSignpostingPageContentAsync("an-example-slug");

        Assert.AreSame(fakeSignpostingPageContent, entry);
    }

    [TestMethod]
    public async Task FetchSignpostingPageContentAsync__TransformsAdjacentEntryReferencesIntoGroupsOfCards()
    {
        var fakeSignpostingPageContent = new SignpostingPageContent {
            Body = new Document {
                Content = new List<IContent> {
                    new Paragraph(),
                    new EntryStructure {
                        Data = new EntryStructureData {
                            Target = new ExternalResourceContent { Title = "Card 1" },
                        },
                    },
                    new Paragraph(),
                    new EntryStructure {
                        Data = new EntryStructureData {
                            Target = new ExternalResourceContent { Title = "Card 2" },
                        },
                    },
                    new EntryStructure {
                        Data = new EntryStructureData {
                            Target = new ExternalResourceContent { Title = "Card 3" },
                        },
                    },
                },
            },
        };

        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<SignpostingPageContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<SignpostingPageContent> {
                Items = new SignpostingPageContent[] { fakeSignpostingPageContent },
            });

        var contentModelTransformerMock = new Mock<IContentModelTransformer>();
        contentModelTransformerMock.Setup(mock => mock.TransformContentToModel<CardModel>(It.IsAny<ExternalResourceContent>()))
            .Returns<ExternalResourceContent>(content => new CardModel {
                Heading = content.Title,
            });

        var contentfulHubContentFetcher = new ContentfulSignpostingPageContentFetcher(contentfulClientMock.Object, contentModelTransformerMock.Object);

        var entry = (await contentfulHubContentFetcher.FetchSignpostingPageContentAsync("an-example-slug"))!;

        Assert.AreEqual(4, entry.Body!.Content.Count);

        TypeAssert.IsType<Paragraph>(entry.Body.Content[0]);

        var cardGroupModel1 = TypeAssert.IsType<CardGroupModel>(entry.Body.Content[1]);
        Assert.AreEqual(1, cardGroupModel1.Cards.Count);
        Assert.AreEqual("Card 1", cardGroupModel1.Cards[0].Heading);

        TypeAssert.IsType<Paragraph>(entry.Body.Content[2]);

        var cardGroupModel2 = TypeAssert.IsType<CardGroupModel>(entry.Body.Content[3]);
        Assert.AreEqual(2, cardGroupModel2.Cards.Count);
        Assert.AreEqual("Card 2", cardGroupModel2.Cards[0].Heading);
        Assert.AreEqual("Card 3", cardGroupModel2.Cards[1].Heading);
    }

    #endregion
}
