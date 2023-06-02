namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;

[TestClass]
public sealed class CustomContentTypeResolverTests
{
    #region Type? Resolve(string)

    [TestMethod]
    public void Resolve__ReturnsNull__WhenContentTypeIdIsUnknown()
    {
        var customContentTypeResolver = new CustomContentTypeResolver();

        var resolvedContentType = customContentTypeResolver.Resolve("someUnexpectedContentTypeId");

        Assert.IsNull(resolvedContentType);
    }

    [DataRow(ContentTypeConstants.ExternalResource, typeof(ExternalResourceContent))]
    [DataRow(ContentTypeConstants.SignpostingPage, typeof(SignpostingPageContent))]
    [DataRow(ContentTypeConstants.Page, typeof(PageContent))]
    [DataRow(ContentTypeConstants.WebsiteGlobals, typeof(WebsiteGlobalsContent))]
    [DataTestMethod]
    public void Resolve__ReturnsExpectedTypeForGivenContentTypeId(string contentTypeId, Type expectedContentType)
    {
        var customContentTypeResolver = new CustomContentTypeResolver();

        var resolvedContentType = customContentTypeResolver.Resolve(contentTypeId);

        Assert.AreSame(expectedContentType, resolvedContentType);
    }

    #endregion
}
