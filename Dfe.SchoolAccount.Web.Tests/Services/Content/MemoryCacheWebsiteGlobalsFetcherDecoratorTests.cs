namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Dfe.SchoolAccount.Web.Configuration;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;

[TestClass]
public sealed class MemoryCacheWebsiteGlobalsFetcherDecoratorTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenEntryCacheOptionsArgumentIsNull()
    {
        var memoryCacheMock = new Mock<IMemoryCache>();
        var innerWebsiteGlobalsModelFetcherMock = new Mock<IWebsiteGlobalsFetcher>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsFetcherDecorator(
                entryCacheOptions: null!,
                memoryCache: memoryCacheMock.Object,
                inner: innerWebsiteGlobalsModelFetcherMock.Object
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("entryCacheOptions", exception.ParamName);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenMemoryCacheArgumentIsNull()
    {
        var entryCacheOptionsMock = new Mock<IOptionsMonitor<EntryCacheOptions>>();
        var innerWebsiteGlobalsModelFetcherMock = new Mock<IWebsiteGlobalsFetcher>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsFetcherDecorator(
                entryCacheOptions: entryCacheOptionsMock.Object,
                memoryCache: null!,
                inner: innerWebsiteGlobalsModelFetcherMock.Object
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("memoryCache", exception.ParamName);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenInnerArgumentIsNull()
    {
        var entryCacheOptionsMock = new Mock<IOptionsMonitor<EntryCacheOptions>>();
        var memoryCacheMock = new Mock<IMemoryCache>();

        var act = () => {
            _ = new MemoryCacheWebsiteGlobalsFetcherDecorator(
                entryCacheOptions: entryCacheOptionsMock.Object,
                memoryCache: memoryCacheMock.Object,
                inner: null!
            );
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("inner", exception.ParamName);
    }

    #endregion

    #region Task<IWebsiteGlobalsModel> FetchWebsiteGlobalsAsync()

    private static MemoryCacheWebsiteGlobalsFetcherDecorator CreateMemoryCacheWebsiteGlobalsFetcherDecorator(out WebsiteGlobalsModel innerValue, out MemoryCache memoryCache)
    {
        innerValue = new WebsiteGlobalsModel();
        memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        var entryCacheOptionsMock = new Mock<IOptionsMonitor<EntryCacheOptions>>();
        entryCacheOptionsMock.Setup(mock => mock.Get(It.IsAny<string>()))
            .Returns(new EntryCacheOptions());

        var innerWebsiteGlobalsModelFetcherMock = new Mock<IWebsiteGlobalsFetcher>();
        innerWebsiteGlobalsModelFetcherMock.Setup(mock => mock.FetchWebsiteGlobalsAsync())
            .ReturnsAsync(innerValue);

        return new MemoryCacheWebsiteGlobalsFetcherDecorator(
            entryCacheOptions: entryCacheOptionsMock.Object,
            memoryCache,
            inner: innerWebsiteGlobalsModelFetcherMock.Object
        );
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsAsync__ReturnsValueFromInnerServiceAndCachesIt__WhenCalledForFirstTime()
    {
        var memoryCacheDecorator = CreateMemoryCacheWebsiteGlobalsFetcherDecorator(out var innerValue, out var memoryCache);

        var value = await memoryCacheDecorator.FetchWebsiteGlobalsAsync();
        Assert.AreSame(innerValue, value);

        object cachedValue = memoryCache.Get(MemoryCacheWebsiteGlobalsFetcherDecorator.MemoryCacheKey);
        Assert.AreSame(innerValue, cachedValue);
    }

    [TestMethod]
    public async Task FetchWebsiteGlobalsAsync__ReturnsValueFromCache__WhenCalledAgain()
    {
        var cachedValue = new WebsiteGlobalsModel();

        var memoryCacheDecorator = CreateMemoryCacheWebsiteGlobalsFetcherDecorator(out _, out var memoryCache);
        memoryCache.Set(MemoryCacheWebsiteGlobalsFetcherDecorator.MemoryCacheKey, cachedValue);

        var value = await memoryCacheDecorator.FetchWebsiteGlobalsAsync();
        Assert.AreSame(cachedValue, value);
    }

    #endregion
}
