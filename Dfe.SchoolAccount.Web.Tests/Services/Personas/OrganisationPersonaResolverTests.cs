namespace Dfe.SchoolAccount.Web.Tests.Services.Personas;

using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Models;
using Dfe.SchoolAccount.Web.Services.Personas;
using Dfe.SchoolAccount.Web.Tests.Helpers;

[TestClass]
public sealed class OrganisationTypePersonaResolverTests
{
    #region PersonaName ResolvePersona(ClaimsPrincipal)

    [TestMethod]
    public void ResolvePersona__ThrowsArgumentNullException__WhenPrincipalArgumentIsNull()
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();

        var act = () => {
            organisationTypePersonaResolver.ResolvePersona(null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [TestMethod]
    public void ResolvePersona__ReturnsUnknownPersona__WhenOrganisationIsNotDefined()
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();
        var user = new ClaimsPrincipal();

        var persona = organisationTypePersonaResolver.ResolvePersona(user);

        Assert.AreEqual(PersonaName.Unknown, persona);
    }

    [TestMethod]
    public void ResolvePersona__ReturnsLaMaintainedSchoolUser__ForUsersOfCommunitySchoolEstablishments()
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();
        var user = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();

        var persona = organisationTypePersonaResolver.ResolvePersona(user);

        Assert.AreEqual(PersonaName.LaMaintainedSchoolUser, persona);
    }

    [DataRow(EstablishmentType.VoluntaryAidedSchool)]
    [DataRow(EstablishmentType.VoluntaryControlledSchool)]
    [DataRow(EstablishmentType.FoundationSchool)]
    [DataRow(EstablishmentType.CityTechnologyCollege)]
    [DataRow(EstablishmentType.CommunitySpecialSchool)]
    [DataRow(EstablishmentType.NonMaintainedSpecialSchool)]
    [DataRow(EstablishmentType.FoundationSpecialSchool)]
    [DataRow(EstablishmentType.PupilReferralUnit)]
    [DataRow(EstablishmentType.AcademySponserLed)]
    [DataRow(EstablishmentType.AcademySpecialSponserLed)]
    [DataRow(EstablishmentType.AcademyConverter)]
    [DataRow(EstablishmentType.FreeSchools)]
    [DataRow(EstablishmentType.FreeSchoolsSpecial)]
    [DataRow(EstablishmentType.FreeSchoolsAlternativeProvision)]
    [DataRow(EstablishmentType.AcademyAlternativeProvisionConverter)]
    [DataRow(EstablishmentType.AcademyAlternativeProvisionSponserLed)]
    [DataRow(EstablishmentType.AcademySpecialConverter)]
    [DataTestMethod]
    public void ResolvePersona__ReturnsLaMaintainedSchoolUser__ForUsersOfAcademySchoolEstablishments(EstablishmentType establishmentType)
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();
        var user = UserFakesHelper.CreateFakeAuthenticatedEstablishmentUser(
            establishmentTypeId: (int)establishmentType,
            establishmentName: establishmentType.ToString()
        );

        var persona = organisationTypePersonaResolver.ResolvePersona(user);

        Assert.AreEqual(PersonaName.AcademySchoolUser, persona);
    }

    [DataRow(EstablishmentType.OtherIndependentSpecialSchool)]
    [DataRow(EstablishmentType.OtherIndependentSchool)]
    [DataRow(EstablishmentType.LaNurserySchool)]
    [DataRow(EstablishmentType.FurtherEducation)]
    [DataRow(EstablishmentType.SecureUnits)]
    [DataRow(EstablishmentType.OffshoreSchools)]
    [DataRow(EstablishmentType.ServiceChildrensEducation)]
    [DataRow(EstablishmentType.Miscellaneous)]
    [DataRow(EstablishmentType.HigherEducationInstitution)]
    [DataRow(EstablishmentType.WelshEstablishment)]
    [DataRow(EstablishmentType.SixthFormCentres)]
    [DataRow(EstablishmentType.SpecialPost16Institution)]
    [DataRow(EstablishmentType.BritishOverseasSchools)]
    [DataRow(EstablishmentType.FreeSchools16To19)]
    [DataRow(EstablishmentType.UniversityTechnicalCollege)]
    [DataRow(EstablishmentType.StudioSchools)]
    [DataRow(EstablishmentType.Academy16To19Converter)]
    [DataRow(EstablishmentType.Academy16To19SponserLed)]
    [DataRow(EstablishmentType.ChildrensCentre)]
    [DataRow(EstablishmentType.ChildrensCentreLinkedSite)]
    [DataRow(EstablishmentType.InstitutionFundedByOtherGovernmentDepartment)]
    [DataRow(EstablishmentType.AcademySecure16To19)]
    [DataTestMethod]
    public void ResolvePersona__ReturnsUnknown__ForUnknownEstablishments(EstablishmentType establishmentType)
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();
        var user = UserFakesHelper.CreateFakeAuthenticatedEstablishmentUser(
            establishmentTypeId: (int)establishmentType,
            establishmentName: establishmentType.ToString()
        );

        var persona = organisationTypePersonaResolver.ResolvePersona(user);

        Assert.AreEqual(PersonaName.Unknown, persona);
    }

    [DataRow(OrganisationCategory.MultiAcademyTrust)]
    [DataRow(OrganisationCategory.SingleAcademyTrust)]
    [DataRow(OrganisationCategory.SecureSat)]
    [DataTestMethod]
    public void ResolvePersona__ReturnsAcademyUser__ForAcademyTrustOrganisationUsers(OrganisationCategory organisationCategory)
    {
        var organisationTypePersonaResolver = new OrganisationTypePersonaResolver();
        var user = UserFakesHelper.CreateFakeAuthenticatedOrganisationUser(
            categoryId: (int)organisationCategory,
            categoryName: organisationCategory.ToString()
        );

        var persona = organisationTypePersonaResolver.ResolvePersona(user);

        Assert.AreEqual(PersonaName.AcademyTrustUser, persona);
    }

    #endregion
}
