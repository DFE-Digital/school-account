namespace Dfe.SchoolAccount.SignIn;

public interface IDfeSignInConfiguration
{
    string Authority { get; }

    string MetaDataUrl { get; }

    /// <summary>
    /// Gets the client ID of the service.
    /// </summary>
    string ClientId { get; }

    /// <summary>
    /// Gets the client secret which is required for interacting with DfE sign-in.
    /// </summary>
    string ClientSecret { get; }

    /// <summary>
    /// Absolute URL Path. Set ONLY if Required By Middleware
    /// </summary>
    string APIServiceProxyUrl { get; }

    string CallbackUrl { get; }

    int CookieExpiration { get; }

    string CookieName { get; }

    bool GetClaimsFromUserInfoEndpoint { get; }

    bool SaveTokens { get; }

    IList<string> Scopes { get; }

    string SignoutCallbackUrl { get; }

    string SignoutRedirectUrl { get; }

    bool SlidingExpiration { get; }

    bool DiscoverRolesWithPublicApi { get; }
}
