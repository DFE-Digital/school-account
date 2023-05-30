namespace Dfe.SchoolAccount.Web.Tests.Services.ContentHyperlinks;

using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Moq;

[TestClass]
public sealed class ContentHyperlinkResolverExtensionsTests
{
    #region ContentHyperlink[] ResolveContentHyperlinks(IContentHyperlinkResolver, IEnumerable<object>)

    [TestMethod]
    public void ResolveContentHyperlinks__ThrowsArgumentNullException__WhenContentHyperlinkResolverIsNull()
    {
        var act = () => {
            ContentHyperlinkResolverExtensions.ResolveContentHyperlinks(null!, new object[0]);
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("contentHyperlinkResolver", exception.ParamName);
    }

    [TestMethod]
    public void ResolveContentHyperlinks__ThrowsArgumentNullException__WhenContentIsNull()
    {
        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();

        var act = () => {
            ContentHyperlinkResolverExtensions.ResolveContentHyperlinks(contentHyperlinkResolverMock.Object, null!);
        };

        var exception = Assert.ThrowsException<ArgumentNullException>(act);
        Assert.AreEqual("content", exception.ParamName);
    }

    [TestMethod]
    public void ResolveContentHyperlinks__ResolvesContentHyperlinkForEachEntryInEumerableCollection()
    {
        var fakeContentCollection = new[] {
            new ExternalResourceContent(),
            new ExternalResourceContent(),
        };
        var fakeContentHyperlinks = new[] {
            new ContentHyperlink(),
            new ContentHyperlink(),
        };

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        contentHyperlinkResolverMock.SetupSequence(mock => mock.ResolveContentHyperlink(It.IsIn(fakeContentCollection)))
            .Returns(fakeContentHyperlinks[0])
            .Returns(fakeContentHyperlinks[1]);

        var contentHyperlinks = ContentHyperlinkResolverExtensions.ResolveContentHyperlinks(contentHyperlinkResolverMock.Object, fakeContentCollection);

        CollectionAssert.AreEqual(fakeContentHyperlinks, contentHyperlinks);
    }

    [TestMethod]
    public void ResolveContentHyperlinks__OmitsUnresolvedContentHyperlinks()
    {
        var fakeContentCollection = new[] {
            new ExternalResourceContent(),
            new ExternalResourceContent(),
            new ExternalResourceContent(),
        };
        var fakeContentHyperlinks = new[] {
            new ContentHyperlink(),
            new ContentHyperlink(),
        };

        var contentHyperlinkResolverMock = new Mock<IContentHyperlinkResolver>();
        contentHyperlinkResolverMock.SetupSequence(mock => mock.ResolveContentHyperlink(It.IsIn(fakeContentCollection)))
            .Returns(fakeContentHyperlinks[0])
            .Returns((ContentHyperlink?)null)
            .Returns(fakeContentHyperlinks[1]);

        var contentHyperlinks = ContentHyperlinkResolverExtensions.ResolveContentHyperlinks(contentHyperlinkResolverMock.Object, fakeContentCollection);

        CollectionAssert.AreEqual(fakeContentHyperlinks, contentHyperlinks);
    }

    #endregion
}
