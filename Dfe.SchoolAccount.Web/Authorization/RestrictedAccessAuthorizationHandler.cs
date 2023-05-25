namespace Dfe.SchoolAccount.Web.Authorization;

using System.Threading.Tasks;
using Dfe.SchoolAccount.SignIn.Extensions;
using Dfe.SchoolAccount.Web.Constants;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// An authorization handler which only permits access for users who are in permitted
/// organisations.
/// </summary>
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

            if (organisation == null) {
                context.Fail(new AuthorizationFailureReason(this, AuthorizationFailureConstants.UserHasNoNoOrganisation));
                return Task.CompletedTask;
            }

            if (!this.configuration.PermittedOrganisationIds.Contains(organisation.Id)) {
                context.Fail(new AuthorizationFailureReason(this, AuthorizationFailureConstants.UserCannotAccessService));
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}
