namespace Dfe.SchoolAccount.Web.Authorization;

using System.Threading.Tasks;
using Dfe.SchoolAccount.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

public sealed class FailedAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly ILogger<FailedAuthorizationMiddlewareResultHandler> logger;
    private readonly IAuthorizationMiddlewareResultHandler defaultHandler;

    public FailedAuthorizationMiddlewareResultHandler(ILogger<FailedAuthorizationMiddlewareResultHandler> logger, IAuthorizationMiddlewareResultHandler defaultHandler)
    {
        this.logger = logger;
        this.defaultHandler = defaultHandler;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.AuthorizationFailure != null) {
            this.logger.LogInformation($"Authorization failed with the reason '{authorizeResult.AuthorizationFailure.FailureReasons.FirstOrDefault()?.Message}'");

            if (authorizeResult.AuthorizationFailure.FailureReasons.Any(reason => reason.Message == AuthorizationFailureConstants.YourInstitutionIsNotYetEligibleForThisService)) {
                context.Response.Redirect($"/{ErrorPageConstants.YourInstitutionIsNotYetEligibleForThisServiceHandle}");
                return;
            }
        }

        await this.defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
