namespace Dfe.SchoolAccount.SignIn.PublicApi;

using Dfe.SchoolAccount.SignIn.Models;

public interface IDfePublicApi
{
    /// <summary>
    /// Get user access information for the service.
    /// </summary>
    /// <seealso href="https://github.com/DFE-Digital/login.dfe.public-api#get-user-access-to-service">Get user access to service</seealso>
    /// <param name="userId">The DfE Sign-in identifier for the user.</param>
    /// <param name="organisationId">The DfE Sign-in identifier for the organisation.</param>
    /// <returns>
    /// An object representing the user's access to the service.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="userId"/> or <paramref name="organisationId"/> is <c>null</c>.
    /// </exception>
    Task<UserAccessToService> GetUserAccessToService(string userId, string organisationId);
}
