namespace Dfe.SchoolAccount.Web.Services.Personas;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.SignIn.Models;

public sealed class OrganisationTypePersonaResolver : IPersonaResolver
{
    private static readonly HashSet<EstablishmentType> ACADEMY_ESTABLISHMENT_TYPES = new HashSet<EstablishmentType> {
        EstablishmentType.VoluntaryAidedSchool,
        EstablishmentType.VoluntaryControlledSchool,
        EstablishmentType.FoundationSchool,
        EstablishmentType.CityTechnologyCollege,
        EstablishmentType.CommunitySpecialSchool,
        EstablishmentType.NonMaintainedSpecialSchool,
        EstablishmentType.FoundationSpecialSchool,
        EstablishmentType.PupilReferralUnit,
        EstablishmentType.AcademySponserLed,
        EstablishmentType.AcademySpecialSponserLed,
        EstablishmentType.AcademyConverter,
        EstablishmentType.FreeSchools,
        EstablishmentType.FreeSchoolsSpecial,
        EstablishmentType.FreeSchoolsAlternativeProvision,
        EstablishmentType.AcademyAlternativeProvisionConverter,
        EstablishmentType.AcademyAlternativeProvisionSponserLed,
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

        if (!principal.HasClaim(claim => claim.Type == ClaimConstants.Organisation)) {
            return PersonaName.Unknown;
        }

        var organisation = principal.GetOrganisation();

        if (organisation.Category.Id == OrganisationCategory.Establishment) {
            if (organisation.Type.Id == EstablishmentType.CommunitySchool) {
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
