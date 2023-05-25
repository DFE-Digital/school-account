namespace Dfe.SchoolAccount.Web.Constants;

public static class AuthorizationFailureConstants
{
    public const string UserHasNoNoOrganisation = "User does not have an organisation with their account";
    public const string UserCannotAccessService = "User organisation does not currently have access to the service";
    public const string YourInstitutionIsNotYetEligibleForThisService = "Your institution is not yet eligible for this service";
}
