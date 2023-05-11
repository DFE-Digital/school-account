namespace Dfe.SchoolAccount.SignIn.Tests.Helpers;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Extensions;

[TestClass]
public sealed class ClaimExtensionsTests
{
    #region Organisation GetOrganisation(this ClaimsPrincipal)

    [TestMethod]
    public void GetOrganisation_ThrowsArgumentNullException_WhenPrincipalArgumentIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => {
            ClaimExtensions.GetOrganisation(null!);
        });
    }

    [TestMethod]
    public void GetOrganisation_ThrowsInvalidOperationException_WhenPrincipalHasNoOrganisationClaim()
    {
        Assert.ThrowsException<InvalidOperationException>(() => {
            var principal = new ClaimsPrincipal();

            ClaimExtensions.GetOrganisation(principal);
        });
    }

    [TestMethod]
    public void GetOrganisation_ReturnsOrganisation()
    {
        var claims = new List<Claim>() {
            new Claim(ClaimConstants.Organisation, @"{
                    ""id"": ""00000000-0000-0000-0000-000000000001"",
                    ""name"": ""An example organisation name"",
                    ""category"": {
                        ""id"": ""category-1234"",
                        ""name"": ""An example category""
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
        Assert.AreEqual("category-1234", organisation.Category.Id);
        Assert.AreEqual("An example category", organisation.Category.Name);
        Assert.IsNull(organisation.Urn);
        Assert.IsNull(organisation.Uid);
        Assert.AreEqual("00000002", organisation.Ukprn);
        Assert.AreEqual("12345678", organisation.LegacyId);
        Assert.AreEqual("sid123456", organisation.Sid);
        Assert.AreEqual("code1234", organisation.DistrictAdministrative_Code);
    }

    #endregion

}
