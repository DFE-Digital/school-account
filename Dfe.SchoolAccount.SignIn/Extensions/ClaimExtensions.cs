namespace Dfe.SchoolAccount.SignIn.Extensions;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Helpers;
using Dfe.SchoolAccount.SignIn.Models;

/// <summary>
/// Extension methods for claim related types.
/// </summary>
public static class ClaimExtensions
{
    /// <summary>
    /// Gets the id claim of the user.
    /// </summary>
    /// <returns>
    /// The user id.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="principal"/> is <c>null</c>
    /// </exception>
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null) {
            throw new ArgumentNullException(nameof(principal));
        }

        return principal.Claims
            .Where(c => c.Type.Contains(ClaimConstants.NameIdentifier))
            .Select(c => c.Value)
            .Single();
    }

    /// <summary>
    /// Gets the organisation of a user by deserializing the organisation claim.
    /// </summary>
    /// <returns>
    /// The deserialized <see cref="Organisation"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="principal"/> is <c>null</c>
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If <paramref name="principal"/> doesn't contain the organisation claim.
    /// </exception>
    public static Organisation GetOrganisation(this ClaimsPrincipal principal)
    {
        if (principal == null) {
            throw new ArgumentNullException(nameof(principal));
        }

        var organisationJson = principal.Claims.Where(c => c.Type == ClaimConstants.Organisation)
            .Select(c => c.Value)
            .FirstOrDefault();

        if (organisationJson == null) {
            throw new InvalidOperationException("Organisation was not set.");
        }

        return JsonHelpers.Deserialize<Organisation>(organisationJson)!;
    }
}
