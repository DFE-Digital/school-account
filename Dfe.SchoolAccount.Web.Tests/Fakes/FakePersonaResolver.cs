namespace Dfe.SchoolAccount.Web.Tests.Fakes;

using System.Security.Claims;
using Dfe.SchoolAccount.Web.Services.Personas;

public sealed class FakePersonaResolver : IPersonaResolver
{
    private readonly PersonaName resolvedPersona;

    public FakePersonaResolver(PersonaName resolvedPersona)
    {
        this.resolvedPersona = resolvedPersona;
    }

    public PersonaName ResolvePersona(ClaimsPrincipal principal)
    {
        return this.resolvedPersona;
    }
}
