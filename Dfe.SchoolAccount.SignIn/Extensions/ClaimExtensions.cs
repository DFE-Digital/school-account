namespace Dfe.SchoolAccount.SignIn.Extensions;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Helpers;
using Dfe.SchoolAccount.SignIn.Models;

public static class ClaimExtensions
{
    public static Organisation GetOrganisation(this ClaimsPrincipal principal)
    {
        var organisationJson = principal.Claims.Where(c => c.Type == ClaimConstants.Organisation)
            .Select(c => c.Value)
            .FirstOrDefault();

        if (organisationJson == null) {
            throw new InvalidOperationException("Organisation was not set.");
        }

        return JsonHelpers.Deserialize<Organisation>(organisationJson)!;
    }
}
