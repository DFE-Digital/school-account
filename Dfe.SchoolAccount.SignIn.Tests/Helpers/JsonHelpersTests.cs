namespace Dfe.SchoolAccount.SignIn.Tests.Helpers;

using Dfe.SchoolAccount.SignIn.Helpers;

[TestClass]
public sealed class JsonHelpersTests
{
    private sealed class ExampleData
    {
        public string ExampleString { get; set; } = null!;
    }

    #region string Serialize<T>(T)

    [TestMethod]
    public void Serialize_T__ReturnsJsonWithCamelCaseNamingOnProperties()
    {
        var exampleData = new ExampleData {
            ExampleString = "abc",
        };

        string json = JsonHelpers.Serialize(exampleData);

        StringAssert.Contains(json, "exampleString", StringComparison.InvariantCulture);
    }

    #endregion

    #region T? Deserialize<T>(string)

    [TestMethod]
    public void Deserialize_T__PopulatesProperties_WhenPropertiesHavePascalCaseNaming()
    {
        var exampleJson = @"{
                ""exampleString"": ""abc""
            }";

        var exampleData = JsonHelpers.Deserialize<ExampleData>(exampleJson)!;

        Assert.AreEqual("abc", exampleData.ExampleString);
    }

    #endregion
}
