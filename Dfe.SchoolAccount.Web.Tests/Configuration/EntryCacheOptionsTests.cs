namespace Dfe.SchoolAccount.Web.Tests.Configuration;

using Dfe.SchoolAccount.Web.Configuration;

[TestClass]
public sealed class EntryCacheOptionsTests
{
    #region MemoryCacheEntryOptions ToMemoryCacheEntryOptions()

    [TestMethod]
    public void ToMemoryCacheEntryOptions__PopulatesSlidingExpirationPropertyWithExpectedValue()
    {
        var entryCacheOptions = new EntryCacheOptions {
            SlidingExpirationInSeconds = 12,
        };

        var memoryCacheEntryOptions = entryCacheOptions.ToMemoryCacheEntryOptions();

        Assert.AreEqual(12, memoryCacheEntryOptions.SlidingExpiration!.Value.TotalSeconds);
    }

    [TestMethod]
    public void ToMemoryCacheEntryOptions__PopulatesAbsoluteExpirationRelativeToNowPropertyWithExpectedValue()
    {
        var entryCacheOptions = new EntryCacheOptions {
            AbsoluteExpirationInSeconds = 123,
        };

        var memoryCacheEntryOptions = entryCacheOptions.ToMemoryCacheEntryOptions();

        Assert.AreEqual(123, memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow!.Value.TotalSeconds);
    }

    #endregion
}
