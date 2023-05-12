namespace Dfe.SchoolAccount.SignIn.Tests.Helpers;

using System.Net;
using System.Text;
using Dfe.SchoolAccount.SignIn.PublicApi;
using Dfe.SchoolAccount.SignIn.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class DfePublicApiTests
{
    private const string FAKE_USER_ID = "00000000-0000-1111-0000-000000000001";
    private const string FAKE_ORGANISATION_ID = "00000000-0000-0000-2222-000000000001";
    private const string FAKE_SERVICE_ID = "00000000-0000-0000-3333-000000000001";
    private const string FAKE_ROLE_ID = "00000000-0000-0000-4444-000000000001";

    private static DfePublicApiConfiguration CreatePublicApiConfigurationMock()
    {
        return new DfePublicApiConfiguration {
            BaseUrl = "https://api.localhost:12345/",
            ApiSecret = "some-example-api-secret",
            ClientId = "someclientid",
            ServiceAudience = "signin.education.gov.uk",
        };
    }

    #region Constructor(IDfePublicApiConfiguration, HttpClient)

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException_WhenConfigurationArgumentIsNull()
    {
        var httpClient = HttpClientMocks.CreateHttpClientMock();

        var act = () => {
            _ = new DfePublicApi(null!, httpClient);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentNullException_WhenHttpClientArgumentIsNull()
    {
        var configuration = CreatePublicApiConfigurationMock();

        var act = () => {
            _ = new DfePublicApi(configuration, null!);
        };

        Assert.ThrowsException<ArgumentNullException>(act);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentException_WhenBaseUrlIsMissing()
    {
        var configuration = CreatePublicApiConfigurationMock();
        configuration.BaseUrl = "";

        var httpClient = HttpClientMocks.CreateHttpClientMock();

        var act = () => {
            _ = new DfePublicApi(configuration, httpClient);
        };

        Assert.ThrowsException<ArgumentException>(act);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentException_WhenApiSecretIsMissing()
    {
        var configuration = CreatePublicApiConfigurationMock();
        configuration.ApiSecret = "";

        var httpClient = HttpClientMocks.CreateHttpClientMock();

        var act = () => {
            _ = new DfePublicApi(configuration, httpClient);
        };

        Assert.ThrowsException<ArgumentException>(act);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentException_WhenClientIdIsMissing()
    {
        var configuration = CreatePublicApiConfigurationMock();
        configuration.ClientId = "";

        var httpClient = HttpClientMocks.CreateHttpClientMock();

        var act = () => {
            _ = new DfePublicApi(configuration, httpClient);
        };

        Assert.ThrowsException<ArgumentException>(act);
    }

    [TestMethod]
    public void Constructor__ThrowsArgumentException_WhenServiceAudienceIsMissing()
    {
        var configuration = CreatePublicApiConfigurationMock();
        configuration.ServiceAudience = "";

        var httpClient = HttpClientMocks.CreateHttpClientMock();

        var act = () => {
            _ = new DfePublicApi(configuration, httpClient);
        };

        Assert.ThrowsException<ArgumentException>(act);
    }

    #endregion

    #region Task<UserAccessToService> GetUserAccessToService(string, string)

    [TestMethod]
    public async Task GetUserAccessToService__ThrowsArgumentNullException_WhenUserIdArgumentIsNull()
    {
        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock();
        var publicApi = new DfePublicApi(configuration, httpClient);

        var act = async () => {
            _ = await publicApi.GetUserAccessToService(null!, FAKE_ORGANISATION_ID);
        };

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(act);
    }

    [TestMethod]
    public async Task GetUserAccessToService__ThrowsArgumentNullException_WhenOrganisationIdArgumentIsNull()
    {
        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock();
        var publicApi = new DfePublicApi(configuration, httpClient);

        var act = async () => {
            _ = await publicApi.GetUserAccessToService(FAKE_USER_ID, null!);
        };

        await Assert.ThrowsExceptionAsync<ArgumentNullException>(act);
    }

    [TestMethod]
    public async Task GetUserAccessToService__InitiatesHttpRequestOnExpectedEndpoint()
    {
        string actualRequestUri = null!;

        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock(request => {
            actualRequestUri = request.RequestUri!.ToString();
            
            var response = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
            return response;
        });
        var publicApi = new DfePublicApi(configuration, httpClient);

        _ = await publicApi.GetUserAccessToService(FAKE_USER_ID, FAKE_ORGANISATION_ID);

        string expectedRequestUri = "https://api.localhost:12345/services/someclientid/organisations/00000000-0000-0000-2222-000000000001/users/00000000-0000-1111-0000-000000000001";
        Assert.AreEqual(actualRequestUri, expectedRequestUri);
    }

    [DataRow(HttpStatusCode.BadRequest)]
    [DataRow(HttpStatusCode.Forbidden)]
    [DataTestMethod]
    public async Task GetUserAccessToService__ThrowsDfePublicApiException_WhenApiRespondsWithFailCode(HttpStatusCode mockStatusCode)
    {
        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock(request => {
            var response = new HttpResponseMessage(mockStatusCode) {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
            return response;
        });
        var publicApi = new DfePublicApi(configuration, httpClient);

        var act = async () => {
            _ = await publicApi.GetUserAccessToService(FAKE_USER_ID, FAKE_ORGANISATION_ID);
        };

        await Assert.ThrowsExceptionAsync<DfePublicApiException>(act);
    }

    [TestMethod]
    public async Task GetUserAccessToService__ReturnsNullWhenUserIsNotEnrolledIntoService()
    {
        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock(request => {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound) {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
            return response;
        });
        var publicApi = new DfePublicApi(configuration, httpClient);

        var userAccessToService = await publicApi.GetUserAccessToService(FAKE_USER_ID, FAKE_ORGANISATION_ID);

        Assert.IsNull(userAccessToService);
    }

    [TestMethod]
    public async Task GetUserAccessToService__ReturnsPopulatedUserAccessToServiceObject()
    {
        var configuration = CreatePublicApiConfigurationMock();
        var httpClient = HttpClientMocks.CreateHttpClientMock(request => {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(@"{
                ""userId"": ""00000000-0000-1111-0000-000000000001"",
                ""serviceId"": ""00000000-0000-0000-3333-000000000001"",
                ""organisationId"": ""00000000-0000-0000-2222-000000000001"",
                ""roles"": [
                    {
                        ""id"": ""00000000-0000-0000-4444-000000000001"",
                        ""name"": ""fake role name"",
                        ""code"": ""fake-role-code"",
                        ""numericId"": 42,
                        ""status"": {
                            ""id"": 1234
                        }
                    }
                ],
                ""identifiers"": [
                    {
                        ""key"": ""fake-identifier"",
                        ""value"": ""123456""
                    }
                ],
                ""status"": [
                    {
                        ""id"": 1234
                    }
                ]
            }", Encoding.UTF8, "application/json");
            return response;
        });
        var publicApi = new DfePublicApi(configuration, httpClient);

        var userAccessToService = await publicApi.GetUserAccessToService(FAKE_USER_ID, FAKE_ORGANISATION_ID);

        Assert.AreEqual(new Guid(FAKE_USER_ID), userAccessToService.UserId);
        Assert.AreEqual(new Guid(FAKE_SERVICE_ID), userAccessToService.ServiceId);
        Assert.AreEqual(new Guid(FAKE_ORGANISATION_ID), userAccessToService.OrganisationId);
        Assert.AreEqual(new Guid(FAKE_ROLE_ID), userAccessToService.Roles[0].Id);
        Assert.AreEqual("fake role name", userAccessToService.Roles[0].Name);
        Assert.AreEqual("fake-role-code", userAccessToService.Roles[0].Code);
        Assert.AreEqual(42, userAccessToService.Roles[0].NumericId);
        Assert.AreEqual(1234, userAccessToService.Roles[0].Status.Id);
        Assert.AreEqual("fake-identifier", userAccessToService.Identifiers[0].Key);
        Assert.AreEqual("123456", userAccessToService.Identifiers[0].Value);
        Assert.AreEqual("123456", userAccessToService.Identifiers[0].Value);
        Assert.AreEqual(1234, userAccessToService.Status[0].Id);
    }

    #endregion
}
