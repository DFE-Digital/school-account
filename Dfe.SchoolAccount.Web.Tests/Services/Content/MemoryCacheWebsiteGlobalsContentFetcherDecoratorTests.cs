namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Dfe.SchoolAccount.Web.Configuration;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

[TestClass]
public sealed class MemoryCacheWebsiteGlobalsContentFetcherDecoratorTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenEntryCacheOptionsArgumentIsNull()
    {
        var memoryCacheMock = new Mock<IMemoryCache>();
        var innerWebsiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsContentFetcherDecorator(
                entryCacheOptions: null!,
                memoryCache: memoryCacheMock.Object,
                inner: innerWebsiteGlobalsContentFetcherMock.Object
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("entryCacheOptions", exception.ParamName);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenMemoryCacheArgumentIsNull()
    {
        var entryCacheOptionsMock = new Mock<IOptionsSnapshot<EntryCacheOptions>>();
        var innerWebsiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsContentFetcherDecorator(
                entryCacheOptions: entryCacheOptionsMock.Object,
                memoryCache: null!,
                inner: innerWebsiteGlobalsContentFetcherMock.Object
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("memoryCache", exception.ParamName);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenInnerArgumentIsNull()
    {
        var entryCacheOptionsMock = new Mock<IOptionsSnapshot<EntryCacheOptions>>();
        var memoryCacheMock = new Mock<IMemoryCache>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsContentFetcherDecorator(
                entryCacheOptions: entryCacheOptionsMock.Object,
                memoryCache: memoryCacheMock.Object,
                inner: null!
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("inner", exception.ParamName);
    }

    #endregion

    #region Task<WebsiteGlobalsContent> FetchWebsiteGlobalsContentAsync()

    [TestMethod]
    public async Task FetchWebsiteGlobalsContentAsync__ReturnsValueFromInnerServiceAndCachesIt__WhenCalledForFirstTime()
    {
        var innerValue = new WebsiteGlobalsContent();

        var entryCacheOptionsMock = new Mock<IOptionsSnapshot<EntryCacheOptions>>();
        entryCacheOptionsMock.Setup(mock => mock.Get(It.IsAny<string>()))
            .Returns(new EntryCacheOptions());

        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        var innerWebsiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();
        innerWebsiteGlobalsContentFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsContentAsync())
            .ReturnsAsync(innerValue);

        var memoryCacheDecorator = new MemoryCacheWebsiteGlobalsContentFetcherDecorator(
            entryCacheOptions: entryCacheOptionsMock.Object,
            memoryCache,
            inner: innerWebsiteGlobalsContentFetcherMock.Object
        );

        var value = await memoryCacheDecorator.FetchWebsiteGlobalsContentAsync();
        Assert.AreSame(innerValue, value);

        object cachedValue = memoryCache.Get(MemoryCacheWebsiteGlobalsContentFetcherDecorator.MemoryCacheKey);
        Assert.AreSame(innerValue, cachedValue);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsContentAsync__ReturnsValueFromCache__WhenCalledAgain()
    {
        var cachedValue = new WebsiteGlobalsContent();
        var innerValue = new WebsiteGlobalsContent();

        var entryCacheOptionsMock = new Mock<IOptionsSnapshot<EntryCacheOptions>>();
        entryCacheOptionsMock.Setup(mock => mock.Get(It.IsAny<string>()))
            .Returns(new EntryCacheOptions());

        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        memoryCache.Set(MemoryCacheWebsiteGlobalsContentFetcherDecorator.MemoryCacheKey, cachedValue);

        var innerWebsiteGlobalsContentFetcherMock = new Mock<IWebsiteGlobalsContentFetcher>();
        innerWebsiteGlobalsContentFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsContentAsync())
            .ReturnsAsync(innerValue);

        var memoryCacheDecorator = new MemoryCacheWebsiteGlobalsContentFetcherDecorator(
            entryCacheOptions: entryCacheOptionsMock.Object,
            memoryCache,
            inner: innerWebsiteGlobalsContentFetcherMock.Object
        );

        var value = await memoryCacheDecorator.FetchWebsiteGlobalsContentAsync();
        Assert.AreSame(cachedValue, value);
    }

    #endregion
}
