namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

[TestClass]
public sealed class HomeControllerTests
{
    private static HomeController CreateHomeControllerWithCommunitySchoolUser()
    {
        var logger = new NullLogger<HomeController>();
        var fakeUser = UserFakesHelper.CreateFakeAuthenticatedCommunitySchoolUser();
        return new HomeController(logger) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext {
                    User = fakeUser,
                },
            }
        };
    }

    #region IActionResult Index()

    [TestMethod]
    public void Index__ReturnsDefaultView()
    {
        var homeController = CreateHomeControllerWithCommunitySchoolUser();

        var result = homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public void Index__PopulatesViewModelWithOrganisationName()
    {
        var homeController = CreateHomeControllerWithCommunitySchoolUser();

        var result = homeController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<HomeViewModel>(viewResult.Model);
        Assert.AreEqual("An example organisation name", model.OrganisationName);
    }

    #endregion
}
