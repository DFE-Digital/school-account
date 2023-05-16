namespace Dfe.SchoolAccount.Web.Authorization;

using System.Threading.Tasks;
using Dfe.SchoolAccount.SignIn.Extensions;
using Microsoft.AspNetCore.Authorization;

public sealed class RestrictedAccessAuthorizationHandler : IAuthorizationHandler
{
    private readonly IRestrictedAccessConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestrictedAccessAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="configuration">Configuration options.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="configuration"/> is <c>null</c>.
    /// </exception>
    public RestrictedAccessAuthorizationHandler(IRestrictedAccessConfiguration configuration)
    {
        if (configuration == null) {
            throw new ArgumentNullException(nameof(configuration));
        }

        this.configuration = configuration;
    }

    /// <inheritdoc/>
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true) {
            var organisation = context.User.GetOrganisation();

            if (!this.configuration.PermittedOrganisationIds.Contains(organisation.Id)) {
                string reason = "User organisation does not currently have access to the service.";
                context.Fail(new AuthorizationFailureReason(this, reason));
            }
        }

        return Task.CompletedTask;
    }
}
