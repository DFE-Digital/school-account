namespace Dfe.SchoolAccount.Web.Controllers;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

public sealed class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;

    public AccountController(ILogger<AccountController> logger)
    {
        this.logger = logger;
    }

    [HttpGet]
    [Route("/account/sign-out")]
    public IActionResult Logout()
    {
        if (this.User.Identity?.IsAuthenticated == false) {
            return this.RedirectToAction("Index", "Home");
        }

        return this.SignOut(
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        );
    }

    [HttpGet]
    [Route("/account/signed-out")]
    public IActionResult SignedOut()
    {
        return this.View();
    }
}
