namespace Dfe.SchoolAccount.SignIn.DsiClient;

using Dfe.SchoolAccount.SignIn.Constants;

public sealed class DfESignInClientFactory
{
    private readonly IDfESignInConfig _dfeSignInConfiguration;

    public DfESignInClientFactory(IDfESignInConfig dfeSignInConfiguration)
    {
        this._dfeSignInConfiguration = dfeSignInConfiguration;
    }

    public DfESignInClient CreateDfESignInClient(string userId = "", string organizationId = "")
    {
        var dfeSignInClient = new DfESignInClient(new HttpClient());
        dfeSignInClient.ServiceId = this._dfeSignInConfiguration.ClientId;
        dfeSignInClient.ServiceUrl = this._dfeSignInConfiguration.APIServiceUrl;
        dfeSignInClient.UserId = userId;
        dfeSignInClient.OrganisationId = organizationId;

        var tokenData = new TokenData();
        tokenData.Header.Add(HeaderConstants.Type, "JWT");
        var token = new TokenBuilder(new TokenDataSerializer(), tokenData, new TokenEncoder(), new JsonWebAlgorithm())
            .UseAlgorithm(this._dfeSignInConfiguration.Cryptography)
            .ForAudience(this._dfeSignInConfiguration.APIServiceAudience)
            .WithSecretKey(this._dfeSignInConfiguration.APIServiceSecret)
            .Issuer(this._dfeSignInConfiguration.ClientId)
            .CreateToken();

        dfeSignInClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        return dfeSignInClient;
    }
}
