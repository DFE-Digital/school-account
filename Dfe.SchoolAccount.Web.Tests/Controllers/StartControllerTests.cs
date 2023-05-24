namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

[TestClass]
public sealed class StartControllerTests
{
    #region IActionResult Index()

    [TestMethod]
    public void Index__ReturnsDefaultView()
    {
        var logger = new NullLogger<StartController>();
        var startController = new StartController(logger);

        var result = startController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.IsNull(viewResult.ViewName);
    }

    #endregion
}
