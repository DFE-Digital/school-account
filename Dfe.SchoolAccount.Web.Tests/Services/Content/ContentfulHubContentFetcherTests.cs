namespace Dfe.SchoolAccount.Web.Tests.Services.Content;

using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.Personas;
using Moq;

[TestClass]
public sealed class ContentfulHubContentFetcherTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenContentfulClientArgumentIsNull()
    {
        var act = () => {
            _ = new ContentfulHubContentFetcher(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(() => act());
    }

    #endregion

    #region Task<HubContent> FetchHubContentAsync(PersonaName)

    [TestMethod]
    public async Task FetchHubContentAsync__ThrowsUnknownPersonaException__WhenPersonaNameIsUnknown()
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        var contentfulHubContentFetcher = new ContentfulHubContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchHubContentAsync(PersonaName.Unknown);
        };

        await Assert.ThrowsExceptionAsync<UnknownPersonaException>(act);
    }

    [DataRow(PersonaName.AcademyTrustUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.AcademySchoolUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, HubConstants.LaMaintainedSchoolUserHandle)]
    [DataTestMethod]
    public async Task FetchHubContentAsync__ThrowsInvalidOperationException__WhenHubEntryWasNotFound(PersonaName personaName, string expectedHubHandle)
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<HubContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<HubContent> {
                Items = Array.Empty<HubContent>(),
            });

        var contentfulHubContentFetcher = new ContentfulHubContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchHubContentAsync(personaName);
        };

        var actualException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
        Assert.AreEqual($"Hub entry '{expectedHubHandle}' was not found.", actualException.Message);
    }

    [DataRow(PersonaName.AcademyTrustUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.AcademySchoolUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, HubConstants.LaMaintainedSchoolUserHandle)]
    [DataTestMethod]
    public async Task FetchHubContentAsync__ThrowsInvalidOperationException__WhenMultipleHubEntriesWereFound(PersonaName personaName, string expectedHubHandle)
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<HubContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<HubContent> {
                Items = new HubContent[] {
                    new HubContent(),
                    new HubContent(),
                },
            });

        var contentfulHubContentFetcher = new ContentfulHubContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchHubContentAsync(personaName);
        };

        var actualException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
        Assert.AreEqual($"Multiple hub entries were found '{expectedHubHandle}'.", actualException.Message);
    }

    [DataRow(PersonaName.AcademyTrustUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.AcademySchoolUser, HubConstants.AcademySchoolUserHandle)]
    [DataRow(PersonaName.LaMaintainedSchoolUser, HubConstants.LaMaintainedSchoolUserHandle)]
    [DataTestMethod]
    public async Task FetchHubContentAsync__RequestsHubContentWithExpectedHandle(PersonaName personaName, string expectedHubHandle)
    {
        var contentfulClientMock = new Mock<IContentfulClient>();
        contentfulClientMock.Setup(mock => mock.GetEntries(It.IsAny<QueryBuilder<HubContent>>(), default))
            .ReturnsAsync(new ContentfulCollection<HubContent> {
                Items = Array.Empty<HubContent>(),
            });

        var contentfulHubContentFetcher = new ContentfulHubContentFetcher(contentfulClientMock.Object);

        var act = async () => {
            _ = await contentfulHubContentFetcher.FetchHubContentAsync(personaName);
        };

        var actualException = await Assert.ThrowsExceptionAsync<InvalidOperationException>(act);
        Assert.AreEqual($"Hub entry '{expectedHubHandle}' was not found.", actualException.Message);
    }

    #endregion
}
