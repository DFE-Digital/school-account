namespace Dfe.SchoolAccount.Web.Services.Content;

using System.Threading.Tasks;
using Dfe.SchoolAccount.Web.Configuration;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

/// <summary>
/// A service that caches website globals content using <see cref="IMemoryCache"/>.
/// </summary>
public sealed class MemoryCacheWebsiteGlobalsContentFetcherDecorator : IWebsiteGlobalsContentFetcher
{
    internal static readonly object MemoryCacheKey = typeof(WebsiteGlobalsContent);

    private readonly IOptionsSnapshot<EntryCacheOptions> entryCacheOptions;
    private readonly IMemoryCache memoryCache;
    private readonly IWebsiteGlobalsContentFetcher inner;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCacheWebsiteGlobalsContentFetcherDecorator"/> class.
    /// </summary>
    /// <param name="entryCacheOptions">Options for entry memory cache.</param>
    /// <param name="memoryCache">Memory cache service.</param>
    /// <param name="inner">The decorated service.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="entryCacheOptions"/>, <paramref name="memoryCache"/> or <paramref name="inner"/> is <c>null</c>.
    /// </exception>
    public MemoryCacheWebsiteGlobalsContentFetcherDecorator(
        IOptionsSnapshot<EntryCacheOptions> entryCacheOptions,
        IMemoryCache memoryCache,
        IWebsiteGlobalsContentFetcher inner)
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
    public async Task<WebsiteGlobalsContent> FetchWebsiteGlobalsContentAsync()
    {
        if (this.memoryCache.TryGetValue<WebsiteGlobalsContent>(MemoryCacheKey, out var content)) {
            return content;
        }

        var memoryCacheEntryOptions = this.entryCacheOptions.Get(EntryCacheConstants.WebsiteGlobalsContent)
            .ToMemoryCacheEntryOptions();

        var freshContent = await this.inner.FetchWebsiteGlobalsContentAsync();
        return this.memoryCache.Set(MemoryCacheKey, freshContent, memoryCacheEntryOptions);
    }
}
