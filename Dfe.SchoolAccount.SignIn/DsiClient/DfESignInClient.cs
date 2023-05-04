namespace Dfe.SchoolAccount.SignIn.DsiClient;

public sealed class DfESignInClient
{
    public DfESignInClient(HttpClient httpClient)
    {
        this.HttpClient = httpClient;
    }

    public HttpClient HttpClient { get; private set; }

    public string? OrganisationId { get; set; }

    public string? ServiceId { get; set; }

    public string? ServiceUrl { get; set; }

    public string? UserId { get; set; }

    public Uri TargetAddress
    {
        get {
            if (string.IsNullOrEmpty(this.ServiceUrl) | string.IsNullOrEmpty(this.ServiceId) | string.IsNullOrEmpty(this.OrganisationId) | string.IsNullOrEmpty(this.UserId)) {
                throw new MemberAccessException("Required Member(s) not set");
            }
            return new Uri($"{this.ServiceUrl}/services/{this.ServiceId}/organisations/{this.OrganisationId}/users/{this.UserId}");
        }
    }
}
