namespace Dfe.SchoolAccount.Web.Tests.Services;

using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Services.Personas;
using Dfe.SchoolAccount.Web.Tests.Fakes;
using Microsoft.AspNetCore.Authorization;

[TestClass]
public sealed class RestrictToSchoolUsersAuthorizationHandlerTests
{
    #region Constructor

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException__WhenPersonaResolverArgumentIsNull()
    {
        var act = () => {
            _ = new RestrictToSchoolUsersAuthorizationHandler(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    #endregion

    #region Task HandleAsync(AuthorizationHandlerContext)

    [DataRow(PersonaName.AcademyTrustUser)]
    [DataRow(PersonaName.AcademySchoolUser)]
    [DataRow(PersonaName.LaMaintainedSchoolUser)]
    [DataTestMethod]
    public async Task HandleAsync__DoesNotBlockAccess__ForSchoolUser(PersonaName personaName)
    {
        var personaResolver = new FakePersonaResolver(personaName);
        var restrictedAccessAuthorizationHandler = new RestrictToSchoolUsersAuthorizationHandler(personaResolver);

        var user = UserFakesHelper.CreateFakeAuthenticatedEstablishmentUser(1, "Test establishment name");
        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsFalse(authorizationHandlerContext.HasFailed);
    }

    [TestMethod]
    public async Task HandleAsync__BlocksAccess__ForNonSchoolUser()
    {
        var personaResolver = new FakePersonaResolver(PersonaName.Unknown);
        var restrictedAccessAuthorizationHandler = new RestrictToSchoolUsersAuthorizationHandler(personaResolver);

        var user = UserFakesHelper.CreateFakeAuthenticatedOrganisationUser(123, "Test organisation name");
        var authorizationRequirements = Array.Empty<IAuthorizationRequirement>();
        var authorizationHandlerContext = new AuthorizationHandlerContext(authorizationRequirements, user, null);

        await restrictedAccessAuthorizationHandler.HandleAsync(authorizationHandlerContext);

        Assert.IsTrue(authorizationHandlerContext.HasFailed);
    }

    #endregion
}
