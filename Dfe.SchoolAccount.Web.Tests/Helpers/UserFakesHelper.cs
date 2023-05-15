namespace Dfe.SchoolAccount.Web.Tests.Helpers;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;

public static class UserFakesHelper
{
    public static ClaimsPrincipal CreateFakeAuthenticatedUser()
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }

    public static ClaimsPrincipal CreateFakeAuthenticatedCommunitySchoolUser()
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "SomeValueHere"),
            new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
            new Claim(ClaimConstants.Organisation, @"{
                ""id"": ""00000000-0000-0000-0000-000000000001"",
                ""name"": ""An example organisation name"",
                ""category"": {
                    ""id"": 1,
                    ""name"": ""Establishment""
                },
                ""type"": {
                    ""id"": 1,
                    ""name"": ""Community School""
                }
            }"),
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));
    }
}
