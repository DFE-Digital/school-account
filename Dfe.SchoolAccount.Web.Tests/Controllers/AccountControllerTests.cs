namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Tests.Fakes;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

[TestClass]
public sealed class AccountControllerTests
{
    #region IActionResult Logout() - GET: /account/sign-out

    [TestMethod]
    public void Logout__RedirectsToStartPage__WhenUserIsNotAuthenticated()
    {
        var logger = new NullLogger<AccountController>();
        var accountController = new AccountController(logger);

        var result = accountController.Logout();

        var redirectToActionResult = TypeAssert.IsType<RedirectToActionResult>(result);
        Assert.AreEqual("Start", redirectToActionResult.ControllerName);
        Assert.AreEqual("Index", redirectToActionResult.ActionName);
    }

    [TestMethod]
    public void Logout__ReturnsExpectedSignOutResult()
    {
        var logger = new NullLogger<AccountController>();
        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedUser();
        var accountController = new AccountController(logger) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext {
                    User = fakeUser,
                },
            }
        };

        var result = accountController.Logout();

        var signOutResult = TypeAssert.IsType<SignOutResult>(result);
        CollectionAssert.Contains(signOutResult.AuthenticationSchemes.ToArray(), "Cookies");
        CollectionAssert.Contains(signOutResult.AuthenticationSchemes.ToArray(), "OpenIdConnect");
    }

    #endregion

    #region IActionResult SignedOut()

    [TestMethod]
    public void SignedOut__ReturnsView__WhenUserIsNotAuthenticated()
    {
        var logger = new NullLogger<AccountController>();
        var accountController = new AccountController(logger);

        var result = accountController.SignedOut();

        TypeAssert.IsType<ViewResult>(result);
    }

    [TestMethod]
    public void SignedOut__RedirectsToHomePage__WhenUserIsAuthenticated()
    {
        var logger = new NullLogger<AccountController>();
        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedUser();
        var accountController = new AccountController(logger) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext {
                    User = fakeUser,
                },
            }
        };

        var result = accountController.SignedOut();

        var redirectToActionResult = TypeAssert.IsType<RedirectToActionResult>(result);
        Assert.AreEqual("Home", redirectToActionResult.ControllerName);
        Assert.AreEqual("Index", redirectToActionResult.ActionName);
    }

    #endregion
}
