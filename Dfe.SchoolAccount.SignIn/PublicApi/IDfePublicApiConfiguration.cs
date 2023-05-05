namespace Dfe.SchoolAccount.SignIn.PublicApi;

public interface IDfePublicApiConfiguration
{
    /// <summary>
    /// Gets the environment specific base URL for the DfE sign-in public API.
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Gets secret which is required for interacting with the service.
    /// </summary>
    string ServiceSecret { get; }

    /// <summary>
    /// Gets the client ID of the service.
    /// </summary>
    string ClientId { get; }

    /// <summary>
    /// Gets the service audience which is typically "signin.education.gov.uk".
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please Refer GitHub Documentation https://github.com/DFE-Digital/login.dfe.public-api
    /// </para>
    /// </remarks>
    string ServiceAudience { get; }
}
