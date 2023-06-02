namespace Dfe.SchoolAccount.Web.Configuration;

using Microsoft.Extensions.Caching.Memory;

public sealed class EntryCacheOptions
{
    public int? SlidingExpirationInSeconds { get; set; }

    public int? AbsoluteExpirationInSeconds { get; set; }

    public MemoryCacheEntryOptions ToMemoryCacheEntryOptions()
    {
        var options = new MemoryCacheEntryOptions();

        if (this.SlidingExpirationInSeconds != null) {
            options.SlidingExpiration = TimeSpan.FromSeconds(this.SlidingExpirationInSeconds.Value);
        }
        if (this.AbsoluteExpirationInSeconds != null) {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(this.AbsoluteExpirationInSeconds.Value);
        }

        return options;
    }
}
