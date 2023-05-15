namespace Dfe.SchoolAccount.Web.Services.Personas;

using System.Security.Claims;

/// <summary>
/// A service which resolves the persona of a user.
/// </summary>
public interface IPersonaResolver
{
    /// <summary>
    /// Resolves the persona of a user.
    /// </summary>
    /// <param name="principal"><see cref="ClaimsPrincipal"/> for user.</param>
    /// <returns>
    /// The resolved persona name; otherwise, a value of <see cref="PersonaName.Unknown"/>.
    /// </returns>
    PersonaName ResolvePersona(ClaimsPrincipal principal);
}
