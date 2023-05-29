namespace Dfe.SchoolAccount.Web.Services.Personas;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.SignIn.Models;

public sealed class OrganisationTypePersonaResolver : IPersonaResolver
{
    private static readonly HashSet<EstablishmentType> LA_MAINTAINED_ESTABLISHMENT_TYPES = new HashSet<EstablishmentType> {
        EstablishmentType.CommunitySchool,
        EstablishmentType.VoluntaryAidedSchool,
        EstablishmentType.VoluntaryControlledSchool,
        EstablishmentType.FoundationSchool,
        EstablishmentType.CommunitySpecialSchool,
        EstablishmentType.FoundationSpecialSchool,
    };

    private static readonly HashSet<EstablishmentType> ACADEMY_ESTABLISHMENT_TYPES = new HashSet<EstablishmentType> {
        EstablishmentType.CityTechnologyCollege,
        EstablishmentType.NonMaintainedSpecialSchool,
        EstablishmentType.AcademySponserLed,
        EstablishmentType.AcademySpecialSponserLed,
        EstablishmentType.AcademyConverter,
        EstablishmentType.FreeSchools,
        EstablishmentType.FreeSchoolsSpecial,
        EstablishmentType.AcademySpecialConverter,
    };

    private static readonly HashSet<OrganisationCategory> ACADEMY_TRUST_ORGANISATION_CATEGORIES = new HashSet<OrganisationCategory> {
        OrganisationCategory.MultiAcademyTrust,
        OrganisationCategory.SingleAcademyTrust,
        OrganisationCategory.SecureSat,
    };

    /// <inheritdoc/>
    public PersonaName ResolvePersona(ClaimsPrincipal principal)
    {
        if (principal == null) {
            throw new ArgumentNullException(nameof(principal));
        }

        var organisation = principal.GetOrganisation();

        if (organisation == null) {
            return PersonaName.Unknown;
        }

        if (organisation.Category.Id == OrganisationCategory.Establishment) {
            if (LA_MAINTAINED_ESTABLISHMENT_TYPES.Contains(organisation.Type.Id)) {
                return PersonaName.LaMaintainedSchoolUser;
            }
            if (ACADEMY_ESTABLISHMENT_TYPES.Contains(organisation.Type.Id)) {
                return PersonaName.AcademySchoolUser;
            }
        }
        else if (ACADEMY_TRUST_ORGANISATION_CATEGORIES.Contains(organisation.Category.Id)) {
            return PersonaName.AcademyTrustUser;
        }
        
        return PersonaName.Unknown;
    }
}
