namespace Dfe.SchoolAccount.Web.Authorization;
public sealed class RestrictedAccessConfiguration : IRestrictedAccessConfiguration
{
    /// <inheritdoc/>
    public IList<Guid> PermittedOrganisationIds { get; set; } = new List<Guid>();
}
