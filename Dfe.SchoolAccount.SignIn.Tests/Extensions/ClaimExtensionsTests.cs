namespace Dfe.SchoolAccount.SignIn.Tests.Helpers;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.SignIn.Models;

[TestClass]
public sealed class ClaimExtensionsTests
{
    #region string GetUserId(this ClaimsPrincipal)

    [TestMethod]
    public void GetUserId_ThrowsArgumentNullException_WhenPrincipalArgumentIsNull()
    {
        var act = () => {
            _ = ClaimExtensions.GetUserId(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [TestMethod]
    public void GetUserId_ReturnsUserId()
    {
        var claims = new List<Claim>() {
            new Claim(ClaimConstants.NameIdentifier, "00000000-0000-0000-0000-000000000001"),
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var userId = ClaimExtensions.GetUserId(principal);

        Assert.AreEqual("00000000-0000-0000-0000-000000000001", userId);
    }

    #endregion

    #region Organisation GetOrganisation(this ClaimsPrincipal)

    [TestMethod]
    public void GetOrganisation_ThrowsArgumentNullException_WhenPrincipalArgumentIsNull()
    {
        var act = () => {
            _ = ClaimExtensions.GetOrganisation(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [TestMethod]
    public void GetOrganisation_ThrowsInvalidOperationException_WhenPrincipalHasNoOrganisationClaim()
    {
        var principal = new ClaimsPrincipal();

        var act = () => {
            _ = ClaimExtensions.GetOrganisation(principal);
        };

        Assert.ThrowsException<InvalidOperationException>(act);
    }

    [TestMethod]
    public void GetOrganisation_ReturnsOrganisation()
    {
        var claims = new List<Claim>() {
            new Claim(ClaimConstants.Organisation, @"{
                    ""id"": ""00000000-0000-0000-0000-000000000001"",
                    ""name"": ""An example organisation name"",
                    ""category"": {
                        ""id"": 1,
                        ""name"": ""Establishment""
                    },
                    ""type"": {
                        ""id"": 18,
                        ""name"": ""Further Education""
                    },
                    ""urn"": null,
                    ""uid"": null,
                    ""ukprn"": ""00000002"",
                    ""legacyId"": ""12345678"",
                    ""sid"": ""sid123456"",
                    ""DistrictAdministrative_code"": ""code1234""
                }"),
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var organisation = ClaimExtensions.GetOrganisation(principal);
            
        Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000001"), organisation.Id);
        Assert.AreEqual("An example organisation name", organisation.Name);
        Assert.AreEqual(OrganisationCategory.Establishment, organisation.Category.Id);
        Assert.AreEqual("Establishment", organisation.Category.Name);
        Assert.AreEqual(EstablishmentType.FurtherEducation, organisation.Type.Id);
        Assert.AreEqual("Further Education", organisation.Type.Name);
        Assert.IsNull(organisation.Urn);
        Assert.IsNull(organisation.Uid);
        Assert.AreEqual("00000002", organisation.Ukprn);
        Assert.AreEqual("12345678", organisation.LegacyId);
        Assert.AreEqual("sid123456", organisation.Sid);
        Assert.AreEqual("code1234", organisation.DistrictAdministrative_Code);
    }

    #endregion
}
