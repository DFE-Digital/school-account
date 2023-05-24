namespace Dfe.SchoolAccount.Web.Tests.Controllers;

using Dfe.SchoolAccount.Web.Controllers;
using Dfe.SchoolAccount.Web.Models;
using Dfe.SchoolAccount.Web.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

[TestClass]
public sealed class ErrorControllerTests
{
    #region IActionResult Index(int?)

    [TestMethod]
    public void Index__ReturnsDefaultView()
    {
        var logger = new NullLogger<ErrorController>();
        var errorController = new ErrorController(logger);

        var result = errorController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.IsNull(viewResult.ViewName);
    }

    [TestMethod]
    public void Index__AssumesDefaultStatusCodeOf500()
    {
        var logger = new NullLogger<ErrorController>();
        var errorController = new ErrorController(logger);

        var result = errorController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreEqual(500, viewResult.StatusCode);
    }

    [DataRow(403)]
    [DataRow(404)]
    [DataTestMethod]
    public void Index__AssumesTheProvidedStatusCode(int httpStatusCode)
    {
        var logger = new NullLogger<ErrorController>();
        var errorController = new ErrorController(logger);

        var result = errorController.Index(httpStatusCode);

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreEqual(httpStatusCode, viewResult.StatusCode);
    }

    [DataRow(403, "Restricted")]
    [DataRow(404, "NotFound")]
    [DataTestMethod]
    public void Index__ReturnsExpectedView__ForErrorsThatHaveBespokeViews(int statusCode, string expectedViewName)
    {
        var logger = new NullLogger<ErrorController>();
        var errorController = new ErrorController(logger);

        var result = errorController.Index(statusCode);

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        Assert.AreEqual(expectedViewName, viewResult.ViewName);
    }

    [TestMethod]
    public void Index__PopulatesViewModelWithRequestId()
    {
        var logger = new NullLogger<ErrorController>();
        var errorController = new ErrorController(logger) {
            ControllerContext = new ControllerContext {
                HttpContext = new DefaultHttpContext {
                    TraceIdentifier = "fake-request-id-12345",
                },
            }
        };

        var result = errorController.Index();

        var viewResult = TypeAssert.IsType<ViewResult>(result);
        var model = TypeAssert.IsType<ErrorViewModel>(viewResult.Model);
        Assert.AreEqual("fake-request-id-12345", model.RequestId);
    }

    #endregion
}
