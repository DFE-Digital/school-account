namespace Dfe.SchoolAccount.SignIn.PublicApi;

public sealed class DfePublicApiConfiguration : IDfePublicApiConfiguration
{
    /// <inheritdoc/>
    public string BaseUrl { get; set; } = null!;

    /// <inheritdoc/>
    public string ServiceSecret { get; set; } = null!;

    /// <inheritdoc/>
    public string ClientId { get; set; } = null!;

    /// <inheritdoc/>
    public string ServiceAudience { get; set; } = "signin.education.gov.uk";
}
