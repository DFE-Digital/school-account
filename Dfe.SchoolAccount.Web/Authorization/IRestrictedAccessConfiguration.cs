namespace Dfe.SchoolAccount.Web.Authorization;

using System.Collections.Generic;

/// <summary>
/// Configuration for <see cref="RestrictedAccessAuthorizationHandler"/>.
/// </summary>
public interface IRestrictedAccessConfiguration
{
    /// <summary>
    /// Identifies the list of identifiers of organisations who have access to the service.
    /// </summary>
    /// <remarks>
    /// <para>These are organisation GUIDs from the DfE Sign-in service;
    /// eg. "00000000-0000-0000-2222-000000000001"</para>
    /// </remarks>
    IList<Guid> PermittedOrganisationIds { get; }
}
