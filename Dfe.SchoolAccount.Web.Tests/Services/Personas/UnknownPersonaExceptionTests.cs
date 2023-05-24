namespace Dfe.SchoolAccount.Web.Tests.Services.Personas;

using Dfe.SchoolAccount.Web.Services.Personas;

[TestClass]
public sealed class UnknownPersonaExceptionTests
{
    #region string Message { get; }

    [TestMethod]
    public void Message__Get__ReturnsValueThatWasProvidedToConstructor()
    {
        var exception = new UnknownPersonaException("Example error message.");

        Assert.AreEqual("Example error message.", exception.Message);
    }

    [TestMethod]
    public void Message__Get__ReturnsValueThatWasProvidedToConstructor__WhenAnInnerExceptionIsAlsoProvided()
    {
        var innerException = new Exception();
        var exception = new UnknownPersonaException("Example error message.", innerException);

        Assert.AreEqual("Example error message.", exception.Message);
    }

    #endregion

    #region Exception InnerException { get; }

    [TestMethod]
    public void InnerException__Get__ReturnsValueThatWasProvidedToConstructor()
    {
        var innerException = new Exception();
        var exception = new UnknownPersonaException("Example error message.", innerException);

        Assert.AreSame(innerException, exception.InnerException);
    }

    #endregion
}
