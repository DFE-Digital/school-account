namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Dfe.SchoolAccount.Web.Configuration;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

/// <summary>
/// A service that caches website globals using <see cref="IMemoryCache"/>.
/// </summary>
public sealed class MemoryCacheWebsiteGlobalsFetcherDecorator : IWebsiteGlobalsFetcher
{
    internal static readonly object MemoryCacheKey = typeof(WebsiteGlobalsModel);

    private readonly IOptionsMonitor<EntryCacheOptions> entryCacheOptions;
    private readonly IMemoryCache memoryCache;
    private readonly IWebsiteGlobalsFetcher inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCacheWebsiteGlobalsFetcherDecorator"/> class.
    /// </summary>
    /// <param name="entryCacheOptions">Options for entry memory cache.</param>
    /// <param name="memoryCache">Memory cache service.</param>
    /// <param name="inner">The decorated service.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="entryCacheOptions"/>, <paramref name="memoryCache"/> or <paramref name="inner"/> is <c>null</c>.
    /// </exception>
    public MemoryCacheWebsiteGlobalsFetcherDecorator(
        IOptionsMonitor<EntryCacheOptions> entryCacheOptions,
        IMemoryCache memoryCache,
        IWebsiteGlobalsFetcher inner)
    {
        if (entryCacheOptions == null) {
            throw new ArgumentNullException(nameof(entryCacheOptions));
        }
        if (memoryCache == null) {
            throw new ArgumentNullException(nameof(memoryCache));
        }
        if (inner == null) {
            throw new ArgumentNullException(nameof(inner));
        }

        this.entryCacheOptions = entryCacheOptions;
        this.memoryCache = memoryCache;
        this.inner = inner;
    }

    /// <inheritdoc/>
    public async Task<WebsiteGlobalsModel> FetchWebsiteGlobalsAsync()
    {
        if (this.memoryCache.TryGetValue<WebsiteGlobalsModel>(MemoryCacheKey, out var model)) {
            return model;
        }

        var memoryCacheEntryOptions = this.entryCacheOptions.Get(EntryCacheConstants.WebsiteGlobals)
            .ToMemoryCacheEntryOptions();

        var freshModel = await this.inner.FetchWebsiteGlobalsAsync();
        return this.memoryCache.Set(MemoryCacheKey, freshModel, memoryCacheEntryOptions);
    }
}
