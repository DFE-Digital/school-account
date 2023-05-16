namespace Dfe.SchoolAccount.Web.Authorization;

using System.Threading.Tasks;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// An authorization handler which only permits access for school users based on the
/// resolved persona.
/// </summary>
/// <seealso cref="Dfe.SchoolAccount.Web.Services.Personas.IPersonaResolver"/>
public sealed class RestrictToSchoolUsersAuthorizationHandler : IAuthorizationHandler
{
    private readonly IPersonaResolver personaResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestrictToSchoolUsersAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="personaResolver">A service which resolves the persona of a user.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="personaResolver"/> is <c>null</c>.
    /// </exception>
    public RestrictToSchoolUsersAuthorizationHandler(IPersonaResolver personaResolver)
    {
        if (personaResolver == null) {
            throw new ArgumentNullException(nameof(personaResolver));
        }

        this.personaResolver = personaResolver;
    }

    /// <inheritdoc/>
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User != null) {
            var resolvedPersona = this.personaResolver.ResolvePersona(context.User);
            if (resolvedPersona == PersonaName.Unknown) {
                string reason = "Not a school user.";
                context.Fail(new AuthorizationFailureReason(this, reason));
            }
        }

        return Task.CompletedTask;
    }
}
