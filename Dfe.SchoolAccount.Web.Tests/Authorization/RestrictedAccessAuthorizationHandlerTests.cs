namespace Dfe.SchoolAccount.Web.Tests.Services;

using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Tests.Fakes;
using Microsoft.AspNetCore.Authorization;

[TestClass]
public sealed class RestrictedAccessAuthorizationHandlerTests
{
    private const string FAKE_ORGANISATION_WITH_ACCESS_ID = "00000000-0000-0000-0000-000000000001";
    private const string FAKE_ORGANISATION_ID_WHICH_USER_IS_NOT_A_PART_OF = "00000000-0000-0000-0000-000000000002";

    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenConfigurationArgumentIsNull()
    {
        var act = () => {
            _ = new RestrictedAccessAuthorizationHandler(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    #endregion

    #region Task HandleAsync(AuthorizationHandlerContext)

    [TestMethod]
    public async Task HandleAsync__DoesNotBlockAccess__WhenUserHasNotBeenAuthenticated()
    {
        var configuration = new RestrictedAccessConfiguration();
        var restrictedAccessAuthorizationHandler = new RestrictedAccessAuthorizationHandler(configuration);

        var user = UserFakesHelper.CreateFakeNonAuthenticatedUser();

        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsFalse(authorizationHandlerContext.HasFailed);
    }

    [TestMethod]
    public async Task HandleAsync__DoesNotBlockAccess__WhenUserIsMemberOfPermittedOrganisation()
    {
        var configuration = new RestrictedAccessConfiguration {
            PermittedOrganisationIds = new[] {
                new Guid(FAKE_ORGANISATION_WITH_ACCESS_ID),
            },
        };
        var restrictedAccessAuthorizationHandler = new RestrictedAccessAuthorizationHandler(configuration);

        // Fake user is in an organisation with the identifier `FAKE_ORGANISATION_WITH_ACCESS_ID`.
        var user = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();

        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsFalse(authorizationHandlerContext.HasFailed);
    }

    [TestMethod]
    public async Task HandleAsync__BlocksAccess__WhenUserIsNotMemberOfAnyOrganisation()
    {
        var configuration = new RestrictedAccessConfiguration();
        var restrictedAccessAuthorizationHandler = new RestrictedAccessAuthorizationHandler(configuration);

        var user = UserFakesHelper.CreateFakeAuthenticatedUser();

        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsTrue(authorizationHandlerContext.HasFailed);
    }

    [TestMethod]
    public async Task HandleAsync__BlocksAccess__WhenUserIsNotMemberOfPermittedOrganisation()
    {
        var configuration = new RestrictedAccessConfiguration {
            PermittedOrganisationIds = new[] {
                new Guid(FAKE_ORGANISATION_ID_WHICH_USER_IS_NOT_A_PART_OF),
            },
        };
        var restrictedAccessAuthorizationHandler = new RestrictedAccessAuthorizationHandler(configuration);

        // Fake user is in an organisation with the identifier `FAKE_ORGANISATION_WITH_ACCESS_ID`.
        var user = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();

        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsTrue(authorizationHandlerContext.HasFailed);
    }

    #endregion
}
