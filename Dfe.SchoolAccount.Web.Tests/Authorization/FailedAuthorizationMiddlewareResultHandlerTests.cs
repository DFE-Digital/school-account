namespace Dfe.SchoolAccount.Web.Tests.Authorization;

using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

[TestClass]
public sealed class FailedAuthorizationMiddlewareResultHandlerTests
{
    #region Task HandleAsync(RequestDelegate, HttpContext, AuthorizationPolicy, PolicyAuthorizationResult)

    [TestMethod]
    public async Task HandleAsync__RedirectsToYourInstitutionIsNotYetEligibleForThisServicePage__WhenFailureOccurs()
    {
        var logger = new NullLogger<FailedAuthorizationMiddlewareResultHandler>();
        var defaultHandlerMock = new Mock<IAuthorizationMiddlewareResultHandler>();
        var failedAuthorizationMiddlewareResultHandler = new FailedAuthorizationMiddlewareResultHandler(logger, defaultHandlerMock.Object);

        var next = (RequestDelegate)(context => Task.CompletedTask);

        var responseMock = new Mock<HttpResponse>();
        var contextMock = new Mock<HttpContext>();
        contextMock.SetupGet(mock => mock.Response)
            .Returns(responseMock.Object);

        var policy = (AuthorizationPolicy)null!;
        var authorizeResult = PolicyAuthorizationResult.Forbid(
            AuthorizationFailure.Failed(new AuthorizationFailureReason[] {
                new AuthorizationFailureReason(null!, AuthorizationFailureConstants.YourInstitutionIsNotYetEligibleForThisService),
            })
        );

        await failedAuthorizationMiddlewareResultHandler.HandleAsync(next, contextMock.Object, policy, authorizeResult);

        responseMock.Verify(mock => mock.Redirect(It.Is<string>(param => "/your-institution-is-not-yet-eligible-for-this-service" == param)));
    }

    [TestMethod]
    public async Task HandleAsync__RevertsToDefaultHandler__WhenDifferentAuthorizationFailuresHaveOccurred()
    {
        var logger = new NullLogger<FailedAuthorizationMiddlewareResultHandler>();
        var defaultHandlerMock = new Mock<IAuthorizationMiddlewareResultHandler>();
        var failedAuthorizationMiddlewareResultHandler = new FailedAuthorizationMiddlewareResultHandler(logger, defaultHandlerMock.Object);

        var next = (RequestDelegate)(context => Task.CompletedTask);
        var contextMock = new Mock<HttpContext>();
        var policy = (AuthorizationPolicy)null!;
        var authorizeResult = PolicyAuthorizationResult.Forbid(
            AuthorizationFailure.Failed(new AuthorizationFailureReason[] {
                new AuthorizationFailureReason(null!, "Some other reason."),
            })
        );

        await failedAuthorizationMiddlewareResultHandler.HandleAsync(next, contextMock.Object, policy, authorizeResult);

        defaultHandlerMock.Verify(mock => mock.HandleAsync(
            It.Is<RequestDelegate>(param => next == param),
            It.Is<HttpContext>(param => contextMock.Object == param),
            It.Is<AuthorizationPolicy>(param => policy == param),
            It.Is<PolicyAuthorizationResult>(param => authorizeResult == param)
        ));
    }

    [TestMethod]
    public async Task HandleAsync__RevertsToDefaultHandler__WhenNoAuthorizationFailuresHaveOccurred()
    {
        var logger = new NullLogger<FailedAuthorizationMiddlewareResultHandler>();
        var defaultHandlerMock = new Mock<IAuthorizationMiddlewareResultHandler>();
        var failedAuthorizationMiddlewareResultHandler = new FailedAuthorizationMiddlewareResultHandler(logger, defaultHandlerMock.Object);

        var next = (RequestDelegate)(context => Task.CompletedTask);
        var contextMock = new Mock<HttpContext>();
        var policy = (AuthorizationPolicy)null!;
        var authorizeResult = PolicyAuthorizationResult.Success();

        await failedAuthorizationMiddlewareResultHandler.HandleAsync(next, contextMock.Object, policy, authorizeResult);

        defaultHandlerMock.Verify(mock => mock.HandleAsync(
            It.Is<RequestDelegate>(param => next == param),
            It.Is<HttpContext>(param => contextMock.Object == param),
            It.Is<AuthorizationPolicy>(param => policy == param),
            It.Is<PolicyAuthorizationResult>(param => authorizeResult == param)
        ));
    }

    #endregion
}
