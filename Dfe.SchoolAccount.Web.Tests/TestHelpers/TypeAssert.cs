namespace Dfe.SchoolAccount.Web.Tests.Helpers;

public static class TypeAssert
{
    public static T IsType<T>(object? value)
    {
        Assert.IsInstanceOfType<T>(value);
        return (T)value;
    }
}
