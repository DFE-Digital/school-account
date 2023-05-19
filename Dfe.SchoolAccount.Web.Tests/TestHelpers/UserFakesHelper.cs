namespace Dfe.SchoolAccount.Web.Tests.TestHelpers;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;

public static class UserFakesHelper
{
    public static ClaimsPrincipal CreateFakeNonAuthenticatedUser()
    {
        return new ClaimsPrincipal();
    }

    public static ClaimsPrincipal CreateFakeAuthenticatedUser()
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new Claim(ClaimTypes.Name, "someuser@somecompany.com"),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }

    public static ClaimsPrincipal CreateFakeAuthenticatedOrganisationUser(int categoryId, string categoryName)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new Claim(ClaimTypes.Name, "someuser@somecompany.com"),
            new Claim(ClaimConstants.Organisation, @"{
                ""id"": ""00000000-0000-0000-0000-000000000001"",
                ""name"": ""An example organisation name"",
                ""category"": {
                    ""id"": " + categoryId + @",
                    ""name"": """ + categoryName + @"""
                }
            }"),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }

    public static ClaimsPrincipal CreateFakeAuthenticatedEstablishmentUser(int establishmentTypeId, string establishmentName)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new Claim(ClaimTypes.Name, "someuser@somecompany.com"),
            new Claim(ClaimConstants.Organisation, @"{
                ""id"": ""00000000-0000-0000-0000-000000000001"",
                ""name"": ""An example organisation name"",
                ""category"": {
                    ""id"": 1,
                    ""name"": ""Establishment""
                },
                ""type"": {
                    ""id"": """ + establishmentTypeId + @""",
                    ""name"": """ + establishmentName + @"""
                }
            }"),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }

    public static ClaimsPrincipal CreateFakeAuthenticatedCommunitySchoolUser()
    {
        return CreateFakeAuthenticatedEstablishmentUser(
            establishmentTypeId: 1,
            establishmentName: "Community School"
        );
    }
}
